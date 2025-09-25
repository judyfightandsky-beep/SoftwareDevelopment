# 領域模型設計 - 使用者登入及註冊系統

## 1. 聚合根設計 (Aggregate Roots)

### 1.1 User Aggregate (使用者聚合)
**聚合根**: User Entity
**業務職責**: 管理使用者身份、認證資訊和基本資料

#### 聚合內實體和值物件
- **User** (聚合根實體)
- **UserCredential** (實體) - 使用者憑證
- **UserProfile** (實體) - 使用者檔案
- **Email** (值物件) - 電子信箱
- **Password** (值物件) - 密碼
- **Username** (值物件) - 使用者名稱
- **UserStatus** (值物件) - 使用者狀態

#### 不變量 (Invariants)
- 每個使用者必須有唯一的電子信箱
- 密碼必須符合安全政策
- 使用者狀態變更必須遵循業務規則
- 使用者名稱必須唯一且符合格式規範

### 1.2 LoginSession Aggregate (登入會話聚合)
**聚合根**: LoginSession Entity
**業務職責**: 管理使用者登入會話和認證狀態

#### 聚合內實體和值物件
- **LoginSession** (聚合根實體)
- **SessionToken** (值物件) - 會話令牌
- **LoginMethod** (值物件) - 登入方式
- **SessionDuration** (值物件) - 會話持續時間
- **DeviceInfo** (值物件) - 裝置資訊

#### 不變量
- 會話必須有明確的到期時間
- 每個會話必須關聯到有效的使用者
- 同一使用者的並行會話數量限制
- 會話狀態變更必須記錄稽核日誌

### 1.3 QRCodeLogin Aggregate (QR碼登入聚合)
**聚合根**: QRCodeLogin Entity
**業務職責**: 管理QR碼登入流程和狀態

#### 聚合內實體和值物件
- **QRCodeLogin** (聚合根實體)
- **QRCode** (值物件) - QR碼資訊
- **QRCodeStatus** (值物件) - QR碼狀態
- **ExpirationTime** (值物件) - 到期時間
- **ConfirmationCode** (值物件) - 確認碼

#### 不變量
- QR碼必須有唯一識別碼和到期時間
- QR碼狀態變更必須遵循狀態機規則
- 確認操作必須在有效時間內完成
- 每個QR碼只能被使用一次

### 1.4 Role Aggregate (角色聚合)
**聚合根**: Role Entity
**業務職責**: 管理角色定義和權限分配

#### 聚合內實體和值物件
- **Role** (聚合根實體)
- **Permission** (實體) - 權限
- **RoleName** (值物件) - 角色名稱
- **RoleDescription** (值物件) - 角色描述
- **PermissionScope** (值物件) - 權限範圍

#### 不變量
- 角色名稱必須唯一
- 權限分配必須符合授權政策
- 內建角色(訪客、員工、主管)不可刪除
- 權限變更必須記錄稽核日誌

### 1.5 AuditLog Aggregate (稽核日誌聚合)
**聚合根**: AuditLog Entity
**業務職責**: 記錄和管理系統稽核日誌

#### 聚合內實體和值物件
- **AuditLog** (聚合根實體)
- **LogEntry** (實體) - 日誌條目
- **ActionType** (值物件) - 操作類型
- **AuditContext** (值物件) - 稽核上下文
- **Timestamp** (值物件) - 時間戳記

#### 不變量
- 稽核日誌一旦寫入不可修改
- 每個操作必須記錄完整的上下文資訊
- 敏感資料必須適當遮罩
- 日誌保存期限符合法規要求

## 2. 實體設計 (Entities)

### 2.1 User Entity
```csharp
public class User : AggregateRoot<UserId>
{
    public Username Username { get; private set; }
    public Email Email { get; private set; }
    public UserStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }

    private readonly List<UserCredential> _credentials;
    public IReadOnlyList<UserCredential> Credentials => _credentials.AsReadOnly();

    // 建構子、業務方法
}
```

### 2.2 LoginSession Entity
```csharp
public class LoginSession : AggregateRoot<SessionId>
{
    public UserId UserId { get; private set; }
    public SessionToken Token { get; private set; }
    public LoginMethod Method { get; private set; }
    public SessionStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DeviceInfo DeviceInfo { get; private set; }

    // 建構子、業務方法
}
```

### 2.3 QRCodeLogin Entity
```csharp
public class QRCodeLogin : AggregateRoot<QRCodeId>
{
    public QRCode Code { get; private set; }
    public QRCodeStatus Status { get; private set; }
    public ExpirationTime ExpiresAt { get; private set; }
    public UserId? ConfirmedByUserId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // 建構子、業務方法
}
```

## 3. 值物件設計 (Value Objects)

### 3.1 Email 值物件
```csharp
public record Email
{
    public string Value { get; init; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email不能為空");

        if (!IsValidEmailFormat(value))
            throw new ArgumentException("無效的Email格式");

        Value = value.ToLowerInvariant();
    }

    private static bool IsValidEmailFormat(string email) =>
        // Email格式驗證邏輯
}
```

### 3.2 Password 值物件
```csharp
public record Password
{
    public string HashedValue { get; init; }
    public string Salt { get; init; }
    public DateTime CreatedAt { get; init; }

    public static Password Create(string plainPassword)
    {
        ValidatePasswordStrength(plainPassword);
        var salt = GenerateSalt();
        var hashedValue = HashPassword(plainPassword, salt);

        return new Password
        {
            HashedValue = hashedValue,
            Salt = salt,
            CreatedAt = DateTime.UtcNow
        };
    }

    public bool Verify(string plainPassword) =>
        HashPassword(plainPassword, Salt) == HashedValue;
}
```

### 3.3 QRCode 值物件
```csharp
public record QRCode
{
    public string Code { get; init; }
    public string SecretKey { get; init; }

    public QRCode()
    {
        Code = GenerateUniqueCode();
        SecretKey = GenerateSecretKey();
    }

    private static string GenerateUniqueCode() =>
        // 生成唯一QR碼邏輯

    private static string GenerateSecretKey() =>
        // 生成密鑰邏輯
}
```

### 3.4 SessionToken 值物件
```csharp
public record SessionToken
{
    public string Value { get; init; }
    public DateTime CreatedAt { get; init; }

    public SessionToken()
    {
        Value = GenerateSecureToken();
        CreatedAt = DateTime.UtcNow;
    }

    private static string GenerateSecureToken() =>
        // 生成安全令牌邏輯
}
```

### 3.5 UserStatus 值物件
```csharp
public record UserStatus
{
    public UserStatusType Type { get; init; }
    public string? Reason { get; init; }
    public DateTime UpdatedAt { get; init; }

    public static UserStatus Active() => new()
    {
        Type = UserStatusType.Active,
        UpdatedAt = DateTime.UtcNow
    };

    public static UserStatus Suspended(string reason) => new()
    {
        Type = UserStatusType.Suspended,
        Reason = reason,
        UpdatedAt = DateTime.UtcNow
    };
}

public enum UserStatusType
{
    Active,
    Suspended,
    Inactive,
    PendingVerification
}
```

## 4. 聚合設計原則

### 4.1 邊界劃分原則
- **業務完整性**: 每個聚合負責一個完整的業務概念
- **事務邊界**: 聚合邊界即事務邊界
- **一致性保證**: 聚合內強一致性，聚合間最終一致性
- **生命週期管理**: 聚合根負責聚合內所有實體的生命週期

### 4.2 聚合間關聯
- 使用ID引用而非直接物件引用
- 透過領域服務協調跨聚合操作
- 使用領域事件實現聚合間通訊
- 避免大聚合，保持聚合小而專注

### 4.3 聚合載入策略
- 聚合根始終完整載入
- 懶載入vs即時載入根據查詢模式決定
- 使用Repository模式隔離資料存取
- 支援快照和事件溯源模式

## 5. 領域不變量管理

### 5.1 聚合內不變量
```csharp
public class User : AggregateRoot<UserId>
{
    public void UpdateEmail(Email newEmail)
    {
        // 檢查Email唯一性的不變量
        if (_emailUniquenessChecker.IsEmailTaken(newEmail))
            throw new DomainException("Email已被使用");

        var oldEmail = Email;
        Email = newEmail;

        // 發佈領域事件
        AddDomainEvent(new UserEmailChangedEvent(Id, oldEmail, newEmail));
    }
}
```

### 5.2 跨聚合不變量
```csharp
public class AuthenticationService : IDomainService
{
    public async Task<LoginResult> AuthenticateAsync(
        string identifier,
        string password)
    {
        // 跨聚合的業務規則檢查
        var user = await _userRepository.GetByIdentifierAsync(identifier);
        if (user == null)
            return LoginResult.UserNotFound();

        if (!user.IsActive())
            return LoginResult.UserSuspended();

        // 密碼驗證
        if (!user.VerifyPassword(password))
        {
            user.RecordFailedLogin();
            return LoginResult.InvalidPassword();
        }

        // 創建登入會話
        var session = LoginSession.Create(user.Id, LoginMethod.Password);

        return LoginResult.Success(session);
    }
}
```

這個領域模型設計確保了業務邏輯的封裝、不變量的維護，以及聚合間的適當邊界劃分，為系統提供了堅實的業務基礎。