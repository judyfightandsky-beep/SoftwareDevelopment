# 應用層設計 - Use Cases和應用服務

## 1. 應用層架構概覽

應用層作為領域層和展示層之間的協調者，負責用例的編排和執行。它不包含業務邏輯，但協調領域物件來完成特定的業務場景。

### 1.1 應用層職責
- **用例編排**: 協調多個領域物件完成業務場景
- **事務管理**: 定義事務邊界
- **資料轉換**: 在DTOs和領域物件間轉換
- **外部服務協調**: 整合外部系統和服務
- **領域事件處理**: 協調事件的發佈和處理

### 1.2 應用層組成
- **Use Cases**: 具體的業務用例實作
- **Application Services**: 應用服務，提供粗粒度的業務操作
- **DTOs**: 資料傳輸物件
- **Commands/Queries**: CQRS模式的命令和查詢
- **Result Objects**: 操作結果封裝

## 2. 核心Use Cases設計

### 2.1 使用者註冊用例 (RegisterUserUseCase)

#### 用例描述
處理新使用者的註冊流程，包括資料驗證、唯一性檢查、密碼政策驗證和使用者創建。

#### Command定義
```csharp
public class RegisterUserCommand
{
    public string Email { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
    public string ConfirmPassword { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public string IpAddress { get; init; }
    public string UserAgent { get; init; }
}
```

#### Result定義
```csharp
public class RegisterUserResult
{
    public bool IsSuccess { get; init; }
    public UserId UserId { get; init; }
    public List<string> Errors { get; init; } = new();
    public string ConfirmationToken { get; init; }

    public static RegisterUserResult Success(UserId userId, string confirmationToken) =>
        new() { IsSuccess = true, UserId = userId, ConfirmationToken = confirmationToken };

    public static RegisterUserResult Failure(params string[] errors) =>
        new() { IsSuccess = false, Errors = errors.ToList() };
}
```

#### Use Case實作
```csharp
public class RegisterUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IUserUniquenessService _uniquenessService;
    private readonly IPasswordPolicyService _passwordPolicyService;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterUserUseCase> _logger;

    public RegisterUserUseCase(
        IUserRepository userRepository,
        IUserUniquenessService uniquenessService,
        IPasswordPolicyService passwordPolicyService,
        IEmailService emailService,
        IUnitOfWork unitOfWork,
        ILogger<RegisterUserUseCase> logger)
    {
        _userRepository = userRepository;
        _uniquenessService = uniquenessService;
        _passwordPolicyService = passwordPolicyService;
        _emailService = emailService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<RegisterUserResult> ExecuteAsync(RegisterUserCommand command)
    {
        try
        {
            // 1. 輸入驗證
            var validationResult = await ValidateInputAsync(command);
            if (!validationResult.IsValid)
                return RegisterUserResult.Failure(validationResult.Errors.ToArray());

            // 2. 領域物件創建
            var email = new Email(command.Email);
            var username = new Username(command.Username);

            // 3. 唯一性檢查
            var uniquenessResult = await _uniquenessService
                .ValidateUserUniquenessAsync(email, username);

            if (!uniquenessResult.IsValid)
                return RegisterUserResult.Failure(uniquenessResult.Errors.ToArray());

            // 4. 密碼政策驗證
            var passwordResult = await _passwordPolicyService
                .ValidatePasswordAsync(command.Password);

            if (!passwordResult.IsValid)
                return RegisterUserResult.Failure(passwordResult.Errors.ToArray());

            // 5. 創建使用者聚合
            var user = User.Register(
                email,
                username,
                Password.Create(command.Password),
                new UserProfile(command.FirstName, command.LastName, command.DateOfBirth));

            // 6. 生成確認令牌
            var confirmationToken = user.GenerateEmailConfirmationToken();

            // 7. 保存到資料庫
            await _userRepository.SaveAsync(user);
            await _unitOfWork.CommitAsync();

            // 8. 發送確認郵件
            await _emailService.SendConfirmationEmailAsync(
                email.Value,
                username.Value,
                confirmationToken);

            _logger.LogInformation("使用者註冊成功: {UserId}", user.Id);

            return RegisterUserResult.Success(user.Id, confirmationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "使用者註冊失敗: {Email}", command.Email);
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    private async Task<ValidationResult> ValidateInputAsync(RegisterUserCommand command)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(command.Email))
            errors.Add("電子信箱不能為空");

        if (string.IsNullOrWhiteSpace(command.Username))
            errors.Add("使用者名稱不能為空");

        if (string.IsNullOrWhiteSpace(command.Password))
            errors.Add("密碼不能為空");

        if (command.Password != command.ConfirmPassword)
            errors.Add("密碼確認不一致");

        return new ValidationResult
        {
            IsValid = !errors.Any(),
            Errors = errors
        };
    }
}
```

### 2.2 使用者登入用例 (LoginUseCase)

#### Command定義
```csharp
public class LoginCommand
{
    public string Identifier { get; init; } // Email或Username
    public string Password { get; init; }
    public bool RememberMe { get; init; }
    public string IpAddress { get; init; }
    public string UserAgent { get; init; }
    public DeviceFingerprint DeviceFingerprint { get; init; }
}
```

#### Result定義
```csharp
public class LoginResult
{
    public bool IsSuccess { get; init; }
    public UserId UserId { get; init; }
    public SessionId SessionId { get; init; }
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
    public DateTime ExpiresAt { get; init; }
    public List<string> Errors { get; init; } = new();
    public bool RequiresTwoFactorAuth { get; init; }

    public static LoginResult Success(
        UserId userId,
        SessionId sessionId,
        string accessToken,
        string refreshToken,
        DateTime expiresAt) =>
        new()
        {
            IsSuccess = true,
            UserId = userId,
            SessionId = sessionId,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt
        };

    public static LoginResult Failure(params string[] errors) =>
        new() { IsSuccess = false, Errors = errors.ToList() };

    public static LoginResult TwoFactorRequired(UserId userId) =>
        new() { IsSuccess = false, UserId = userId, RequiresTwoFactorAuth = true };
}
```

#### Use Case實作
```csharp
public class LoginUseCase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ITokenService _tokenService;
    private readonly ILoginSessionRepository _sessionRepository;
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LoginUseCase> _logger;

    public async Task<LoginResult> ExecuteAsync(LoginCommand command)
    {
        try
        {
            // 1. 創建認證上下文
            var authContext = new AuthenticationContext
            {
                IpAddress = command.IpAddress,
                UserAgent = command.UserAgent,
                DeviceInfo = DeviceInfo.FromFingerprint(command.DeviceFingerprint)
            };

            // 2. 執行認證
            var authResult = await _authenticationService.AuthenticateAsync(
                command.Identifier,
                command.Password,
                authContext);

            if (!authResult.IsSuccess)
            {
                await LogFailedAttemptAsync(command.Identifier, authResult.Error, authContext);
                return LoginResult.Failure(authResult.Error);
            }

            // 3. 檢查是否需要雙因子認證
            if (authResult.RequiresTwoFactorAuth)
            {
                return LoginResult.TwoFactorRequired(authResult.UserId);
            }

            // 4. 創建登入會話
            var sessionDuration = command.RememberMe
                ? TimeSpan.FromDays(30)
                : TimeSpan.FromHours(8);

            var session = LoginSession.Create(
                authResult.UserId,
                LoginMethod.Password,
                authContext.DeviceInfo,
                sessionDuration);

            await _sessionRepository.SaveAsync(session);

            // 5. 生成令牌
            var tokens = await _tokenService.GenerateTokensAsync(
                authResult.UserId,
                session.Id,
                sessionDuration);

            // 6. 記錄成功登入
            await LogSuccessfulLoginAsync(authResult.UserId, session.Id, authContext);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation("使用者登入成功: {UserId}", authResult.UserId);

            return LoginResult.Success(
                authResult.UserId,
                session.Id,
                tokens.AccessToken,
                tokens.RefreshToken,
                session.ExpiresAt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "登入處理失敗: {Identifier}", command.Identifier);
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    private async Task LogFailedAttemptAsync(
        string identifier,
        string error,
        AuthenticationContext context)
    {
        var auditLog = AuditLog.CreateAuthenticationEvent(
            identifier,
            AuthenticationEventType.Failed,
            error,
            context);

        await _auditLogRepository.SaveAsync(auditLog);
    }

    private async Task LogSuccessfulLoginAsync(
        UserId userId,
        SessionId sessionId,
        AuthenticationContext context)
    {
        var auditLog = AuditLog.CreateAuthenticationEvent(
            userId.ToString(),
            AuthenticationEventType.Success,
            $"會話ID: {sessionId}",
            context);

        await _auditLogRepository.SaveAsync(auditLog);
    }
}
```

### 2.3 QR碼登入用例 (QRCodeLoginUseCase)

#### QR碼生成用例
```csharp
public class GenerateQRCodeForLoginUseCase
{
    private readonly IQRCodeService _qrCodeService;
    private readonly IQRCodeLoginRepository _qrCodeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<QRCodeGenerationResult> ExecuteAsync()
    {
        // 1. 生成QR碼登入實體
        var qrCodeLogin = await _qrCodeService.GenerateQRCodeAsync();

        // 2. 保存到資料庫
        await _qrCodeRepository.SaveAsync(qrCodeLogin);
        await _unitOfWork.CommitAsync();

        // 3. 生成QR碼圖片URL
        var imageUrl = _qrCodeService.GenerateQRCodeImageUrl(qrCodeLogin.Code);

        return QRCodeGenerationResult.Success(
            qrCodeLogin.Id,
            qrCodeLogin.Code.Code,
            imageUrl,
            qrCodeLogin.ExpiresAt.Value);
    }
}
```

#### QR碼確認用例
```csharp
public class ConfirmQRCodeLoginUseCase
{
    private readonly IQRCodeService _qrCodeService;
    private readonly IQRCodeLoginRepository _qrCodeRepository;
    private readonly ILoginSessionRepository _sessionRepository;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<QRCodeConfirmationResult> ExecuteAsync(ConfirmQRCodeCommand command)
    {
        // 1. 驗證QR碼
        var validationResult = await _qrCodeService.ValidateQRCodeAsync(command.QRCodeValue);
        if (!validationResult.IsValid)
            return QRCodeConfirmationResult.Failure(validationResult.Error);

        // 2. 確認QR碼登入
        var confirmationResult = await _qrCodeService.ConfirmQRCodeAsync(
            validationResult.QRCodeId,
            command.UserId);

        if (!confirmationResult.IsSuccess)
            return QRCodeConfirmationResult.Failure(confirmationResult.Error);

        // 3. 創建登入會話
        var session = LoginSession.Create(
            command.UserId,
            LoginMethod.QRCode,
            command.DeviceInfo);

        await _sessionRepository.SaveAsync(session);

        // 4. 生成令牌
        var tokens = await _tokenService.GenerateTokensAsync(
            command.UserId,
            session.Id,
            TimeSpan.FromHours(8));

        await _unitOfWork.CommitAsync();

        return QRCodeConfirmationResult.Success(
            session.Id,
            tokens.AccessToken,
            tokens.RefreshToken,
            session.ExpiresAt);
    }
}
```

## 3. Application Services設計

### 3.1 UserApplicationService
```csharp
public class UserApplicationService
{
    private readonly RegisterUserUseCase _registerUseCase;
    private readonly LoginUseCase _loginUseCase;
    private readonly LogoutUseCase _logoutUseCase;
    private readonly ChangePasswordUseCase _changePasswordUseCase;

    public UserApplicationService(
        RegisterUserUseCase registerUseCase,
        LoginUseCase loginUseCase,
        LogoutUseCase logoutUseCase,
        ChangePasswordUseCase changePasswordUseCase)
    {
        _registerUseCase = registerUseCase;
        _loginUseCase = loginUseCase;
        _logoutUseCase = logoutUseCase;
        _changePasswordUseCase = changePasswordUseCase;
    }

    public async Task<RegisterUserDto> RegisterAsync(RegisterUserRequestDto request)
    {
        var command = MapToCommand(request);
        var result = await _registerUseCase.ExecuteAsync(command);

        return MapToDto(result);
    }

    public async Task<LoginDto> LoginAsync(LoginRequestDto request)
    {
        var command = MapToCommand(request);
        var result = await _loginUseCase.ExecuteAsync(command);

        return MapToDto(result);
    }

    // 其他方法...
}
```

## 4. 資料傳輸物件 (DTOs)

### 4.1 Request DTOs
```csharp
public class RegisterUserRequestDto
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
}

public class LoginRequestDto
{
    public string Identifier { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}
```

### 4.2 Response DTOs
```csharp
public class RegisterUserDto
{
    public string UserId { get; set; }
    public bool IsSuccess { get; set; }
    public List<string> Errors { get; set; } = new();
    public string Message { get; set; }
}

public class LoginDto
{
    public string UserId { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsSuccess { get; set; }
    public List<string> Errors { get; set; } = new();
    public bool RequiresTwoFactorAuth { get; set; }
}
```

## 5. CQRS模式實作

### 5.1 Query端設計
```csharp
public class GetUserProfileQuery
{
    public string UserId { get; init; }
}

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
{
    private readonly IUserReadRepository _userReadRepository;

    public async Task<UserProfileDto> Handle(
        GetUserProfileQuery request,
        CancellationToken cancellationToken)
    {
        var userProfile = await _userReadRepository
            .GetUserProfileAsync(new UserId(request.UserId));

        return MapToDto(userProfile);
    }
}
```

### 5.2 Command端設計
```csharp
public class UpdateUserProfileCommand : IRequest<UpdateUserProfileResult>
{
    public string UserId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime? DateOfBirth { get; init; }
}

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UpdateUserProfileResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<UpdateUserProfileResult> Handle(
        UpdateUserProfileCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(new UserId(request.UserId));
        if (user == null)
            return UpdateUserProfileResult.NotFound();

        user.UpdateProfile(
            request.FirstName,
            request.LastName,
            request.DateOfBirth);

        await _userRepository.SaveAsync(user);
        await _unitOfWork.CommitAsync();

        return UpdateUserProfileResult.Success();
    }
}
```

## 6. 錯誤處理和驗證

### 6.1 Result模式
```csharp
public abstract class Result
{
    public bool IsSuccess { get; protected init; }
    public bool IsFailure => !IsSuccess;
    public List<string> Errors { get; protected init; } = new();

    protected Result() { }

    public static Result Success() => new SuccessResult();
    public static Result Failure(params string[] errors) => new FailureResult(errors);
}

public class Result<T> : Result
{
    public T Data { get; private init; }

    private Result() { }

    public static Result<T> Success(T data) => new()
    {
        IsSuccess = true,
        Data = data
    };

    public new static Result<T> Failure(params string[] errors) => new()
    {
        IsSuccess = false,
        Errors = errors.ToList()
    };
}
```

### 6.2 驗證管道
```csharp
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var validationResults = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(request, cancellationToken)));

        var errors = validationResults
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }

        return await next();
    }
}
```

這個應用層設計確保了業務邏輯的正確協調，提供了清晰的用例邊界，並支援複雜的業務場景處理。