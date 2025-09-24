# 領域服務和領域事件設計

## 1. 領域服務設計 (Domain Services)

領域服務用於處理不自然屬於任何特定實體或值物件的業務邏輯，特別是涉及多個聚合或需要外部資源的操作。

### 1.1 IAuthenticationService (認證服務)

#### 職責範圍
- 跨聚合的使用者認證邏輯
- 密碼驗證和安全政策執行
- 登入嘗試次數控制和帳戶鎖定

#### 介面定義
```csharp
public interface IAuthenticationService
{
    Task<AuthenticationResult> AuthenticateAsync(
        string identifier,
        string password,
        AuthenticationContext context);

    Task<AuthenticationResult> AuthenticateWithQRCodeAsync(
        QRCodeId qrCodeId,
        UserId userId,
        AuthenticationContext context);

    Task<bool> ValidatePasswordStrengthAsync(string password);

    Task<bool> IsAccountLockedAsync(UserId userId);

    Task RecordAuthenticationAttemptAsync(
        UserId userId,
        AuthenticationResult result,
        AuthenticationContext context);
}
```

#### 實作邏輯範例
```csharp
public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly ILoginSessionRepository _loginSessionRepository;
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IPasswordPolicyService _passwordPolicyService;

    public async Task<AuthenticationResult> AuthenticateAsync(
        string identifier,
        string password,
        AuthenticationContext context)
    {
        // 1. 查找使用者
        var user = await _userRepository.GetByIdentifierAsync(identifier);
        if (user == null)
        {
            await RecordFailedAttempt(identifier, "用戶不存在", context);
            return AuthenticationResult.Failure("無效的憑證");
        }

        // 2. 檢查帳戶狀態
        if (!user.IsActive())
        {
            await RecordFailedAttempt(user.Id, "帳戶已停用", context);
            return AuthenticationResult.Failure("帳戶已停用");
        }

        // 3. 檢查帳戶鎖定狀態
        if (await IsAccountLockedAsync(user.Id))
        {
            await RecordFailedAttempt(user.Id, "帳戶已鎖定", context);
            return AuthenticationResult.Failure("帳戶已鎖定");
        }

        // 4. 驗證密碼
        if (!user.VerifyPassword(password))
        {
            user.RecordFailedLogin();
            await _userRepository.SaveAsync(user);
            await RecordFailedAttempt(user.Id, "密碼錯誤", context);
            return AuthenticationResult.Failure("無效的憑證");
        }

        // 5. 成功認證
        user.RecordSuccessfulLogin();
        await _userRepository.SaveAsync(user);

        // 6. 創建登入會話
        var session = LoginSession.Create(
            user.Id,
            LoginMethod.Password,
            context.DeviceInfo);

        await _loginSessionRepository.SaveAsync(session);

        // 7. 記錄成功登入
        await RecordSuccessfulAttempt(user.Id, context);

        return AuthenticationResult.Success(session);
    }

    private async Task RecordFailedAttempt(
        object identifier,
        string reason,
        AuthenticationContext context)
    {
        var auditLog = AuditLog.CreateAuthenticationEvent(
            identifier?.ToString(),
            AuthenticationEventType.Failed,
            reason,
            context);

        await _auditLogRepository.SaveAsync(auditLog);
    }
}
```

### 1.2 IQRCodeService (QR碼服務)

#### 職責範圍
- QR碼的生成和驗證
- QR碼狀態管理
- QR碼安全性控制

#### 介面定義
```csharp
public interface IQRCodeService
{
    Task<QRCodeLogin> GenerateQRCodeAsync();
    Task<QRCodeValidationResult> ValidateQRCodeAsync(string qrCodeValue);
    Task<QRCodeConfirmationResult> ConfirmQRCodeAsync(QRCodeId qrCodeId, UserId userId);
    Task<bool> IsQRCodeExpiredAsync(QRCodeId qrCodeId);
    Task InvalidateQRCodeAsync(QRCodeId qrCodeId);
    string GenerateQRCodeImageUrl(QRCode qrCode);
}
```

### 1.3 IPasswordPolicyService (密碼政策服務)

#### 職責範圍
- 密碼強度驗證
- 密碼政策規則執行
- 密碼歷史檢查

#### 介面定義
```csharp
public interface IPasswordPolicyService
{
    Task<PasswordValidationResult> ValidatePasswordAsync(
        string password,
        UserId? userId = null);

    Task<bool> IsPasswordReusedAsync(UserId userId, string password);

    PasswordPolicy GetCurrentPasswordPolicy();

    Task<TimeSpan> GetPasswordExpirationAsync(UserId userId);
}
```

### 1.4 IUserUniquenessService (使用者唯一性服務)

#### 職責範圍
- 跨聚合的唯一性檢查
- Email和Username重複檢驗
- 使用者識別碼唯一性保證

#### 介面定義
```csharp
public interface IUserUniquenessService
{
    Task<bool> IsEmailUniqueAsync(Email email, UserId? excludeUserId = null);
    Task<bool> IsUsernameUniqueAsync(Username username, UserId? excludeUserId = null);
    Task<UniquenesValidationResult> ValidateUserUniquenessAsync(
        Email email,
        Username username,
        UserId? excludeUserId = null);
}
```

## 2. 領域事件設計 (Domain Events)

領域事件用於解耦聚合間的交互，實現最終一致性，並支援複雜的業務流程協調。

### 2.1 使用者相關事件

#### UserRegisteredEvent
```csharp
public class UserRegisteredEvent : DomainEvent
{
    public UserId UserId { get; }
    public Email Email { get; }
    public Username Username { get; }
    public DateTime RegisteredAt { get; }

    public UserRegisteredEvent(
        UserId userId,
        Email email,
        Username username,
        DateTime registeredAt)
    {
        UserId = userId;
        Email = email;
        Username = username;
        RegisteredAt = registeredAt;
    }
}
```

#### UserLoggedInEvent
```csharp
public class UserLoggedInEvent : DomainEvent
{
    public UserId UserId { get; }
    public SessionId SessionId { get; }
    public LoginMethod LoginMethod { get; }
    public DeviceInfo DeviceInfo { get; }
    public DateTime LoggedInAt { get; }

    public UserLoggedInEvent(
        UserId userId,
        SessionId sessionId,
        LoginMethod loginMethod,
        DeviceInfo deviceInfo,
        DateTime loggedInAt)
    {
        UserId = userId;
        SessionId = sessionId;
        LoginMethod = loginMethod;
        DeviceInfo = deviceInfo;
        LoggedInAt = loggedInAt;
    }
}
```

#### UserLoggedOutEvent
```csharp
public class UserLoggedOutEvent : DomainEvent
{
    public UserId UserId { get; }
    public SessionId SessionId { get; }
    public LogoutReason Reason { get; }
    public DateTime LoggedOutAt { get; }

    public UserLoggedOutEvent(
        UserId userId,
        SessionId sessionId,
        LogoutReason reason,
        DateTime loggedOutAt)
    {
        UserId = userId;
        SessionId = sessionId;
        Reason = reason;
        LoggedOutAt = loggedOutAt;
    }
}
```

#### UserPasswordChangedEvent
```csharp
public class UserPasswordChangedEvent : DomainEvent
{
    public UserId UserId { get; }
    public DateTime ChangedAt { get; }
    public string ChangedByIpAddress { get; }

    public UserPasswordChangedEvent(
        UserId userId,
        DateTime changedAt,
        string changedByIpAddress)
    {
        UserId = userId;
        ChangedAt = changedAt;
        ChangedByIpAddress = changedByIpAddress;
    }
}
```

#### UserEmailChangedEvent
```csharp
public class UserEmailChangedEvent : DomainEvent
{
    public UserId UserId { get; }
    public Email OldEmail { get; }
    public Email NewEmail { get; }
    public DateTime ChangedAt { get; }

    public UserEmailChangedEvent(
        UserId userId,
        Email oldEmail,
        Email newEmail,
        DateTime changedAt)
    {
        UserId = userId;
        OldEmail = oldEmail;
        NewEmail = newEmail;
        ChangedAt = changedAt;
    }
}
```

### 2.2 會話相關事件

#### SessionCreatedEvent
```csharp
public class SessionCreatedEvent : DomainEvent
{
    public SessionId SessionId { get; }
    public UserId UserId { get; }
    public LoginMethod LoginMethod { get; }
    public DateTime CreatedAt { get; }
    public DateTime ExpiresAt { get; }

    public SessionCreatedEvent(
        SessionId sessionId,
        UserId userId,
        LoginMethod loginMethod,
        DateTime createdAt,
        DateTime expiresAt)
    {
        SessionId = sessionId;
        UserId = userId;
        LoginMethod = loginMethod;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
    }
}
```

#### SessionExpiredEvent
```csharp
public class SessionExpiredEvent : DomainEvent
{
    public SessionId SessionId { get; }
    public UserId UserId { get; }
    public DateTime ExpiredAt { get; }

    public SessionExpiredEvent(
        SessionId sessionId,
        UserId userId,
        DateTime expiredAt)
    {
        SessionId = sessionId;
        UserId = userId;
        ExpiredAt = expiredAt;
    }
}
```

### 2.3 QR碼登入事件

#### QRCodeGeneratedEvent
```csharp
public class QRCodeGeneratedEvent : DomainEvent
{
    public QRCodeId QRCodeId { get; }
    public QRCode Code { get; }
    public DateTime GeneratedAt { get; }
    public DateTime ExpiresAt { get; }

    public QRCodeGeneratedEvent(
        QRCodeId qrCodeId,
        QRCode code,
        DateTime generatedAt,
        DateTime expiresAt)
    {
        QRCodeId = qrCodeId;
        Code = code;
        GeneratedAt = generatedAt;
        ExpiresAt = expiresAt;
    }
}
```

#### QRCodeScannedEvent
```csharp
public class QRCodeScannedEvent : DomainEvent
{
    public QRCodeId QRCodeId { get; }
    public UserId ScannedByUserId { get; }
    public DateTime ScannedAt { get; }
    public DeviceInfo DeviceInfo { get; }

    public QRCodeScannedEvent(
        QRCodeId qrCodeId,
        UserId scannedByUserId,
        DateTime scannedAt,
        DeviceInfo deviceInfo)
    {
        QRCodeId = qrCodeId;
        ScannedByUserId = scannedByUserId;
        ScannedAt = scannedAt;
        DeviceInfo = deviceInfo;
    }
}
```

#### QRCodeConfirmedEvent
```csharp
public class QRCodeConfirmedEvent : DomainEvent
{
    public QRCodeId QRCodeId { get; }
    public UserId ConfirmedByUserId { get; }
    public SessionId CreatedSessionId { get; }
    public DateTime ConfirmedAt { get; }

    public QRCodeConfirmedEvent(
        QRCodeId qrCodeId,
        UserId confirmedByUserId,
        SessionId createdSessionId,
        DateTime confirmedAt)
    {
        QRCodeId = qrCodeId;
        ConfirmedByUserId = confirmedByUserId;
        CreatedSessionId = createdSessionId;
        ConfirmedAt = confirmedAt;
    }
}
```

### 2.4 安全事件

#### SecurityViolationEvent
```csharp
public class SecurityViolationEvent : DomainEvent
{
    public string ViolationType { get; }
    public UserId? UserId { get; }
    public string IpAddress { get; }
    public string UserAgent { get; }
    public string Description { get; }
    public SecurityLevel Severity { get; }
    public DateTime OccurredAt { get; }

    public SecurityViolationEvent(
        string violationType,
        string description,
        SecurityLevel severity,
        DateTime occurredAt,
        UserId? userId = null,
        string ipAddress = null,
        string userAgent = null)
    {
        ViolationType = violationType;
        UserId = userId;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        Description = description;
        Severity = severity;
        OccurredAt = occurredAt;
    }
}
```

#### SuspiciousActivityEvent
```csharp
public class SuspiciousActivityEvent : DomainEvent
{
    public UserId UserId { get; }
    public string ActivityType { get; }
    public string Description { get; }
    public RiskLevel RiskLevel { get; }
    public DateTime DetectedAt { get; }
    public Dictionary<string, object> Metadata { get; }

    public SuspiciousActivityEvent(
        UserId userId,
        string activityType,
        string description,
        RiskLevel riskLevel,
        DateTime detectedAt,
        Dictionary<string, object> metadata = null)
    {
        UserId = userId;
        ActivityType = activityType;
        Description = description;
        RiskLevel = riskLevel;
        DetectedAt = detectedAt;
        Metadata = metadata ?? new Dictionary<string, object>();
    }
}
```

## 3. 領域事件處理模式

### 3.1 事件處理器範例

#### UserRegisteredEventHandler
```csharp
public class UserRegisteredEventHandler : INotificationHandler<UserRegisteredEvent>
{
    private readonly IEmailService _emailService;
    private readonly IAuditLogRepository _auditLogRepository;

    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        // 1. 發送歡迎郵件
        await _emailService.SendWelcomeEmailAsync(
            notification.Email.Value,
            notification.Username.Value);

        // 2. 記錄稽核日誌
        var auditLog = AuditLog.CreateUserRegistrationEvent(
            notification.UserId,
            notification.Email,
            notification.RegisteredAt);

        await _auditLogRepository.SaveAsync(auditLog);

        // 3. 其他業務邏輯...
    }
}
```

### 3.2 事件發佈模式

#### 聚合根中的事件發佈
```csharp
public abstract class AggregateRoot<TId> : Entity<TId>
{
    private readonly List<DomainEvent> _domainEvents = new();

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
```

#### Repository中的事件發佈
```csharp
public class UserRepository : IUserRepository
{
    private readonly IMediator _mediator;

    public async Task SaveAsync(User user)
    {
        // 1. 保存實體
        await SaveEntityToDatabase(user);

        // 2. 發佈領域事件
        foreach (var domainEvent in user.DomainEvents)
        {
            await _mediator.Publish(domainEvent);
        }

        // 3. 清除事件
        user.ClearDomainEvents();
    }
}
```

## 4. 領域服務與事件的協作

### 4.1 服務間協作範例
```csharp
public class UserRegistrationDomainService
{
    private readonly IUserUniquenessService _uniquenessService;
    private readonly IPasswordPolicyService _passwordPolicyService;

    public async Task<RegistrationResult> RegisterUserAsync(
        Email email,
        Username username,
        string password)
    {
        // 1. 檢查唯一性
        var uniquenessResult = await _uniquenessService
            .ValidateUserUniquenessAsync(email, username);

        if (!uniquenessResult.IsValid)
            return RegistrationResult.Failure(uniquenessResult.Errors);

        // 2. 驗證密碼政策
        var passwordResult = await _passwordPolicyService
            .ValidatePasswordAsync(password);

        if (!passwordResult.IsValid)
            return RegistrationResult.Failure(passwordResult.Errors);

        // 3. 創建使用者
        var user = User.Register(email, username, Password.Create(password));

        return RegistrationResult.Success(user);
    }
}
```

這種設計確保了領域邏輯的純淨性，同時提供了靈活的事件驅動架構，支援複雜的業務場景和系統集成需求。