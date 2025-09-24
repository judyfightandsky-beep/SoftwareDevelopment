# 介面契約定義 - Repository和Domain Services

## 1. Repository介面設計

Repository介面負責封裝資料存取邏輯，在Domain層定義介面，Infrastructure層實作具體邏輯，遵循依賴反轉原則。

### 1.1 基礎Repository介面

#### IRepository<T, TId> 基礎介面
```csharp
public interface IRepository<T, TId> where T : AggregateRoot<TId>
{
    Task<T> GetByIdAsync(TId id);
    Task<T> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task SaveAsync(T aggregate);
    Task SaveAsync(T aggregate, CancellationToken cancellationToken = default);
    Task DeleteAsync(T aggregate);
    Task DeleteAsync(TId id);
    Task<bool> ExistsAsync(TId id);
}
```

### 1.2 IUserRepository (使用者儲存庫介面)

#### 核心職責
- 使用者聚合的CRUD操作
- 基於不同條件的使用者查詢
- 使用者唯一性檢查
- 批量操作支援

#### 介面定義
```csharp
public interface IUserRepository : IRepository<User, UserId>
{
    // === 基本查詢操作 ===
    Task<User> GetByEmailAsync(Email email);
    Task<User> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);

    Task<User> GetByUsernameAsync(Username username);
    Task<User> GetByUsernameAsync(Username username, CancellationToken cancellationToken = default);

    Task<User> GetByIdentifierAsync(string identifier); // Email or Username
    Task<User> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken = default);

    // === 唯一性檢查 ===
    Task<bool> IsEmailUniqueAsync(Email email, UserId? excludeUserId = null);
    Task<bool> IsUsernameUniqueAsync(Username username, UserId? excludeUserId = null);
    Task<bool> IsIdentifierUniqueAsync(string identifier, UserId? excludeUserId = null);

    // === 查詢集合操作 ===
    Task<IEnumerable<User>> GetUsersByStatusAsync(UserStatus status);
    Task<IEnumerable<User>> GetUsersByRoleAsync(RoleId roleId);
    Task<IEnumerable<User>> GetUsersCreatedBetweenAsync(DateTime startDate, DateTime endDate);

    // === 分頁查詢 ===
    Task<PagedResult<User>> GetUsersPagedAsync(
        int page,
        int pageSize,
        UserSearchCriteria criteria = null);

    Task<PagedResult<UserSummary>> GetUserSummariesPagedAsync(
        int page,
        int pageSize,
        UserSearchCriteria criteria = null);

    // === 統計操作 ===
    Task<int> CountActiveUsersAsync();
    Task<int> CountUsersByStatusAsync(UserStatus status);
    Task<Dictionary<UserStatusType, int>> GetUserStatusStatisticsAsync();

    // === 批量操作 ===
    Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<UserId> userIds);
    Task SaveManyAsync(IEnumerable<User> users);
    Task DeleteManyAsync(IEnumerable<UserId> userIds);

    // === 特殊查詢 ===
    Task<IEnumerable<User>> GetUsersWithExpiredPasswordsAsync();
    Task<IEnumerable<User>> GetInactiveUsersAsync(TimeSpan inactiveThreshold);
    Task<User> GetUserWithCredentialsAsync(UserId userId);
}
```

#### 搜尋條件物件
```csharp
public class UserSearchCriteria
{
    public string SearchTerm { get; init; }
    public UserStatusType? Status { get; init; }
    public RoleId? RoleId { get; init; }
    public DateTime? CreatedAfter { get; init; }
    public DateTime? CreatedBefore { get; init; }
    public DateTime? LastLoginAfter { get; init; }
    public DateTime? LastLoginBefore { get; init; }
    public string SortBy { get; init; } = "CreatedAt";
    public SortDirection SortDirection { get; init; } = SortDirection.Descending;
}

public class UserSummary
{
    public UserId Id { get; init; }
    public string Email { get; init; }
    public string Username { get; init; }
    public UserStatusType Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? LastLoginAt { get; init; }
}
```

### 1.3 ILoginSessionRepository (登入會話儲存庫介面)

#### 核心職責
- 登入會話的生命週期管理
- 會話驗證和清理
- 使用者會話查詢

#### 介面定義
```csharp
public interface ILoginSessionRepository : IRepository<LoginSession, SessionId>
{
    // === 基本查詢操作 ===
    Task<LoginSession> GetByTokenAsync(SessionToken token);
    Task<LoginSession> GetActiveSessionAsync(SessionId sessionId);

    // === 使用者會話管理 ===
    Task<IEnumerable<LoginSession>> GetActiveSessionsByUserAsync(UserId userId);
    Task<IEnumerable<LoginSession>> GetAllSessionsByUserAsync(UserId userId);
    Task<LoginSession> GetLatestSessionByUserAsync(UserId userId);

    // === 會話驗證 ===
    Task<bool> IsSessionActiveAsync(SessionId sessionId);
    Task<bool> IsTokenValidAsync(SessionToken token);

    // === 會話清理 ===
    Task InvalidateSessionAsync(SessionId sessionId);
    Task InvalidateAllUserSessionsAsync(UserId userId);
    Task InvalidateExpiredSessionsAsync();
    Task<int> CleanupExpiredSessionsAsync();

    // === 會話統計 ===
    Task<int> CountActiveSessionsAsync();
    Task<int> CountActiveSessionsByUserAsync(UserId userId);
    Task<Dictionary<LoginMethod, int>> GetLoginMethodStatisticsAsync(DateTime? since = null);

    // === 安全相關 ===
    Task<IEnumerable<LoginSession>> GetSuspiciousSessionsAsync(
        TimeSpan timeWindow,
        int maxSessionsPerUser);

    Task<IEnumerable<LoginSession>> GetSessionsByDeviceAsync(DeviceInfo deviceInfo);
    Task<IEnumerable<LoginSession>> GetSessionsByIpAddressAsync(string ipAddress);

    // === 批量操作 ===
    Task InvalidateSessionsAsync(IEnumerable<SessionId> sessionIds);
    Task<IEnumerable<LoginSession>> GetSessionsExpiringWithinAsync(TimeSpan timeSpan);
}
```

### 1.4 IQRCodeLoginRepository (QR碼登入儲存庫介面)

#### 核心職責
- QR碼登入流程管理
- QR碼狀態追蹤
- 過期QR碼清理

#### 介面定義
```csharp
public interface IQRCodeLoginRepository : IRepository<QRCodeLogin, QRCodeId>
{
    // === 基本查詢操作 ===
    Task<QRCodeLogin> GetByCodeAsync(string code);
    Task<QRCodeLogin> GetActiveQRCodeAsync(QRCodeId id);

    // === QR碼狀態管理 ===
    Task<IEnumerable<QRCodeLogin>> GetQRCodesByStatusAsync(QRCodeStatus status);
    Task<IEnumerable<QRCodeLogin>> GetExpiredQRCodesAsync();
    Task<IEnumerable<QRCodeLogin>> GetQRCodesExpiringWithinAsync(TimeSpan timeSpan);

    // === QR碼驗證 ===
    Task<bool> IsQRCodeValidAsync(string code);
    Task<bool> IsQRCodeExpiredAsync(QRCodeId id);

    // === QR碼清理 ===
    Task InvalidateQRCodeAsync(QRCodeId id);
    Task<int> CleanupExpiredQRCodesAsync();
    Task CleanupQRCodesOlderThanAsync(DateTime threshold);

    // === 統計操作 ===
    Task<int> CountQRCodesByStatusAsync(QRCodeStatus status);
    Task<Dictionary<QRCodeStatus, int>> GetQRCodeStatusStatisticsAsync();

    // === 安全相關 ===
    Task<IEnumerable<QRCodeLogin>> GetQRCodesGeneratedByIpAsync(
        string ipAddress,
        TimeSpan timeWindow);

    Task<int> CountQRCodesGeneratedByIpAsync(
        string ipAddress,
        TimeSpan timeWindow);
}
```

### 1.5 IRoleRepository (角色儲存庫介面)

#### 介面定義
```csharp
public interface IRoleRepository : IRepository<Role, RoleId>
{
    // === 基本查詢操作 ===
    Task<Role> GetByNameAsync(RoleName roleName);
    Task<IEnumerable<Role>> GetAllRolesAsync();
    Task<IEnumerable<Role>> GetSystemRolesAsync();
    Task<IEnumerable<Role>> GetCustomRolesAsync();

    // === 權限相關 ===
    Task<IEnumerable<Role>> GetRolesWithPermissionAsync(PermissionId permissionId);
    Task<IEnumerable<Permission>> GetRolePermissionsAsync(RoleId roleId);

    // === 使用者角色關聯 ===
    Task<IEnumerable<Role>> GetUserRolesAsync(UserId userId);
    Task AssignRoleToUserAsync(UserId userId, RoleId roleId);
    Task RemoveRoleFromUserAsync(UserId userId, RoleId roleId);
    Task<bool> UserHasRoleAsync(UserId userId, RoleId roleId);

    // === 角色層次結構 ===
    Task<IEnumerable<Role>> GetChildRolesAsync(RoleId parentRoleId);
    Task<IEnumerable<Role>> GetParentRolesAsync(RoleId childRoleId);
    Task<bool> IsRoleAncestorAsync(RoleId ancestorRoleId, RoleId descendantRoleId);
}
```

### 1.6 IAuditLogRepository (稽核日誌儲存庫介面)

#### 介面定義
```csharp
public interface IAuditLogRepository : IRepository<AuditLog, AuditLogId>
{
    // === 基本查詢操作 ===
    Task<IEnumerable<AuditLog>> GetLogsByUserAsync(
        UserId userId,
        DateTime? from = null,
        DateTime? to = null);

    Task<IEnumerable<AuditLog>> GetLogsByActionTypeAsync(
        ActionType actionType,
        DateTime? from = null,
        DateTime? to = null);

    // === 分頁查詢 ===
    Task<PagedResult<AuditLog>> GetLogsPagedAsync(
        int page,
        int pageSize,
        AuditLogSearchCriteria criteria = null);

    // === 安全事件查詢 ===
    Task<IEnumerable<AuditLog>> GetSecurityEventsAsync(
        DateTime? from = null,
        DateTime? to = null);

    Task<IEnumerable<AuditLog>> GetFailedLoginAttemptsAsync(
        string identifier,
        TimeSpan timeWindow);

    // === 統計操作 ===
    Task<Dictionary<ActionType, int>> GetActionTypeStatisticsAsync(
        DateTime? from = null,
        DateTime? to = null);

    Task<int> CountEventsByUserAsync(
        UserId userId,
        DateTime? from = null,
        DateTime? to = null);

    // === 清理操作 ===
    Task<int> CleanupOldLogsAsync(DateTime threshold);
    Task ArchiveLogsAsync(DateTime threshold, string archiveLocation);
}
```

## 2. Domain Services介面設計

Domain Services介面定義跨聚合的業務邏輯和外部服務依賴。

### 2.1 IAuthenticationService (認證服務介面)

#### 完整介面定義
```csharp
public interface IAuthenticationService
{
    // === 基本認證操作 ===
    Task<AuthenticationResult> AuthenticateAsync(
        string identifier,
        string password,
        AuthenticationContext context);

    Task<AuthenticationResult> AuthenticateWithQRCodeAsync(
        QRCodeId qrCodeId,
        UserId userId,
        AuthenticationContext context);

    Task<AuthenticationResult> AuthenticateWithTokenAsync(
        SessionToken token,
        AuthenticationContext context);

    // === 密碼管理 ===
    Task<PasswordValidationResult> ValidatePasswordStrengthAsync(
        string password,
        UserId? userId = null);

    Task<bool> IsPasswordReusedAsync(UserId userId, string password);

    Task<PasswordChangeResult> ChangePasswordAsync(
        UserId userId,
        string currentPassword,
        string newPassword);

    // === 帳戶安全 ===
    Task<bool> IsAccountLockedAsync(UserId userId);
    Task LockAccountAsync(UserId userId, string reason, TimeSpan? duration = null);
    Task UnlockAccountAsync(UserId userId);

    // === 認證嘗試記錄 ===
    Task RecordAuthenticationAttemptAsync(
        string identifier,
        AuthenticationResult result,
        AuthenticationContext context);

    Task<int> GetFailedAttemptCountAsync(string identifier, TimeSpan timeWindow);

    // === 雙因子認證 ===
    Task<bool> IsTwoFactorEnabledAsync(UserId userId);
    Task<TwoFactorSetupResult> SetupTwoFactorAsync(UserId userId);
    Task<TwoFactorValidationResult> ValidateTwoFactorCodeAsync(
        UserId userId,
        string code);

    // === 安全策略 ===
    Task<SecurityAssessmentResult> AssessSecurityRiskAsync(
        UserId userId,
        AuthenticationContext context);

    Task<IEnumerable<SecurityRecommendation>> GetSecurityRecommendationsAsync(UserId userId);
}
```

#### 相關資料結構
```csharp
public class AuthenticationResult
{
    public bool IsSuccess { get; init; }
    public UserId UserId { get; init; }
    public string Error { get; init; }
    public bool RequiresTwoFactorAuth { get; init; }
    public LoginSession Session { get; init; }

    public static AuthenticationResult Success(UserId userId, LoginSession session) =>
        new() { IsSuccess = true, UserId = userId, Session = session };

    public static AuthenticationResult Failure(string error) =>
        new() { IsSuccess = false, Error = error };

    public static AuthenticationResult TwoFactorRequired(UserId userId) =>
        new() { IsSuccess = false, UserId = userId, RequiresTwoFactorAuth = true };
}

public class AuthenticationContext
{
    public string IpAddress { get; init; }
    public string UserAgent { get; init; }
    public DeviceInfo DeviceInfo { get; init; }
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    public Dictionary<string, object> AdditionalProperties { get; init; } = new();
}
```

### 2.2 IQRCodeService (QR碼服務介面)

#### 完整介面定義
```csharp
public interface IQRCodeService
{
    // === QR碼生成 ===
    Task<QRCodeLogin> GenerateQRCodeAsync();
    Task<QRCodeLogin> GenerateQRCodeWithCustomExpirationAsync(TimeSpan expiration);

    // === QR碼驗證 ===
    Task<QRCodeValidationResult> ValidateQRCodeAsync(string qrCodeValue);
    Task<bool> IsQRCodeValidAsync(QRCodeId qrCodeId);
    Task<bool> IsQRCodeExpiredAsync(QRCodeId qrCodeId);

    // === QR碼確認 ===
    Task<QRCodeConfirmationResult> ConfirmQRCodeAsync(QRCodeId qrCodeId, UserId userId);
    Task<QRCodeConfirmationResult> ConfirmQRCodeWithDeviceAsync(
        QRCodeId qrCodeId,
        UserId userId,
        DeviceInfo deviceInfo);

    // === QR碼管理 ===
    Task InvalidateQRCodeAsync(QRCodeId qrCodeId);
    Task InvalidateExpiredQRCodesAsync();
    Task<int> CleanupExpiredQRCodesAsync();

    // === QR碼圖片生成 ===
    string GenerateQRCodeImageUrl(QRCode qrCode);
    Task<byte[]> GenerateQRCodeImageAsync(QRCode qrCode, QRCodeImageFormat format = QRCodeImageFormat.PNG);

    // === QR碼統計 ===
    Task<QRCodeUsageStatistics> GetUsageStatisticsAsync(DateTime? from = null, DateTime? to = null);

    // === 安全控制 ===
    Task<bool> IsQRCodeGenerationRateLimitExceededAsync(string ipAddress);
    Task<SecurityAssessmentResult> AssessQRCodeSecurityRiskAsync(
        QRCodeId qrCodeId,
        AuthenticationContext context);
}
```

### 2.3 IPasswordPolicyService (密碼政策服務介面)

#### 完整介面定義
```csharp
public interface IPasswordPolicyService
{
    // === 密碼驗證 ===
    Task<PasswordValidationResult> ValidatePasswordAsync(
        string password,
        UserId? userId = null);

    Task<PasswordStrengthResult> AssessPasswordStrengthAsync(string password);

    // === 密碼歷史 ===
    Task<bool> IsPasswordReusedAsync(UserId userId, string password);
    Task<int> GetPasswordReuseHistoryLengthAsync();
    Task AddPasswordToHistoryAsync(UserId userId, Password password);

    // === 密碪政策 ===
    PasswordPolicy GetCurrentPasswordPolicy();
    Task<PasswordPolicy> GetPasswordPolicyForUserAsync(UserId userId);
    Task UpdatePasswordPolicyAsync(PasswordPolicy policy);

    // === 密碼到期 ===
    Task<bool> IsPasswordExpiredAsync(UserId userId);
    Task<DateTime?> GetPasswordExpirationDateAsync(UserId userId);
    Task<TimeSpan?> GetPasswordExpirationWarningPeriodAsync(UserId userId);

    // === 密碼生成 ===
    Task<string> GenerateSecurePasswordAsync(PasswordGenerationOptions options = null);
    Task<string> GenerateTemporaryPasswordAsync();

    // === 安全建議 ===
    Task<IEnumerable<PasswordSecurityRecommendation>> GetPasswordSecurityRecommendationsAsync(UserId userId);
    Task<bool> IsPasswordCompromisedAsync(string password);
}
```

### 2.4 ITokenService (令牌服務介面)

#### 完整介面定義
```csharp
public interface ITokenService
{
    // === 令牌生成 ===
    Task<TokenPair> GenerateTokensAsync(UserId userId, SessionId sessionId, TimeSpan expiration);
    Task<string> GenerateAccessTokenAsync(UserId userId, SessionId sessionId, TimeSpan expiration);
    Task<string> GenerateRefreshTokenAsync(SessionId sessionId);

    // === 令牌驗證 ===
    Task<TokenValidationResult> ValidateAccessTokenAsync(string token);
    Task<TokenValidationResult> ValidateRefreshTokenAsync(string token);

    // === 令牌更新 ===
    Task<TokenPair> RefreshTokensAsync(string refreshToken);
    Task<string> RefreshAccessTokenAsync(string refreshToken);

    // === 令牌撤銷 ===
    Task RevokeTokenAsync(string token);
    Task RevokeAllUserTokensAsync(UserId userId);
    Task RevokeSessionTokensAsync(SessionId sessionId);

    // === 令牌資訊 ===
    Task<TokenClaimsResult> GetTokenClaimsAsync(string token);
    Task<bool> IsTokenBlacklistedAsync(string token);

    // === 特殊令牌 ===
    Task<string> GenerateEmailConfirmationTokenAsync(UserId userId);
    Task<string> GeneratePasswordResetTokenAsync(UserId userId);
    Task<bool> ValidateEmailConfirmationTokenAsync(string token, UserId userId);
    Task<bool> ValidatePasswordResetTokenAsync(string token, UserId userId);
}
```

## 3. 外部服務介面

### 3.1 IEmailService (郵件服務介面)

#### 介面定義
```csharp
public interface IEmailService
{
    // === 基本郵件發送 ===
    Task SendEmailAsync(string to, string subject, string body);
    Task SendEmailAsync(EmailMessage message);

    // === 範本郵件 ===
    Task SendWelcomeEmailAsync(string email, string username);
    Task SendConfirmationEmailAsync(string email, string username, string confirmationToken);
    Task SendPasswordResetEmailAsync(string email, string resetToken);
    Task SendPasswordChangedNotificationAsync(string email, string username);

    // === 安全通知 ===
    Task SendSecurityAlertEmailAsync(string email, string alertMessage);
    Task SendLoginNotificationEmailAsync(string email, LoginNotificationData data);
    Task SendAccountLockNotificationAsync(string email, string reason);

    // === 批量郵件 ===
    Task SendBulkEmailAsync(IEnumerable<string> recipients, string subject, string body);

    // === 郵件狀態 ===
    Task<EmailDeliveryStatus> GetEmailStatusAsync(string messageId);
    Task<IEnumerable<EmailDeliveryStatus>> GetEmailStatusBatchAsync(IEnumerable<string> messageIds);
}
```

### 3.2 IEncryptionService (加密服務介面)

#### 介面定義
```csharp
public interface IEncryptionService
{
    // === 密碼哈希 ===
    string HashPassword(string password, string salt);
    string GenerateSalt();
    bool VerifyPassword(string password, string hash, string salt);

    // === 資料加密 ===
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
    byte[] Encrypt(byte[] data);
    byte[] Decrypt(byte[] encryptedData);

    // === 數位簽章 ===
    string GenerateSignature(string data);
    bool VerifySignature(string data, string signature);

    // === 隨機值生成 ===
    string GenerateSecureRandomString(int length);
    byte[] GenerateSecureRandomBytes(int length);

    // === 金鑰管理 ===
    Task<string> GetEncryptionKeyAsync(string keyId);
    Task RotateEncryptionKeyAsync(string keyId);
}
```

## 4. 工作單元介面

### 4.1 IUnitOfWork (工作單元介面)

#### 介面定義
```csharp
public interface IUnitOfWork : IDisposable
{
    // === 事務管理 ===
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);

    // === 儲存庫存取 ===
    IUserRepository Users { get; }
    ILoginSessionRepository LoginSessions { get; }
    IQRCodeLoginRepository QRCodeLogins { get; }
    IRoleRepository Roles { get; }
    IAuditLogRepository AuditLogs { get; }

    // === 變更追蹤 ===
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    bool HasChanges();
    void DetachAll();

    // === 事件處理 ===
    Task PublishDomainEventsAsync(CancellationToken cancellationToken = default);
    void ClearDomainEvents();
}
```

這些介面契約確保了系統的高度解耦，支援依賴注入和單元測試，同時提供了清晰的邊界定義和職責劃分。