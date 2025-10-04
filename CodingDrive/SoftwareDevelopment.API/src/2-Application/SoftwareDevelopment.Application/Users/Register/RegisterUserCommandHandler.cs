using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SoftwareDevelopment.Application.Common;
using SoftwareDevelopment.Domain.Users.Entities;
using SoftwareDevelopment.Domain.Users.ValueObjects;
using SoftwareDevelopment.Domain.Users.Repositories;
using SoftwareDevelopment.Domain.Shared;
using SoftwareDevelopment.Domain.Common;

namespace SoftwareDevelopment.Application.Users.Register;

/// <summary>
/// 註冊使用者命令處理器
/// </summary>
public sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Result<RegisterUserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RegisterUserCommandHandler> _logger;

    /// <summary>
    /// 初始化註冊使用者命令處理器
    /// </summary>
    /// <param name="userRepository">使用者儲存庫</param>
    /// <param name="passwordHasher">密碼雜湊服務</param>
    /// <param name="context">資料庫內容</param>
    /// <param name="configuration">配置</param>
    /// <param name="logger">日誌記錄器</param>
    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IApplicationDbContext context,
        IConfiguration configuration,
        ILogger<RegisterUserCommandHandler> logger)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// 處理註冊使用者命令
    /// </summary>
    /// <param name="request">註冊使用者命令</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>註冊結果</returns>
    public async Task<Result<RegisterUserResponse>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing user registration for {Email}", request.Email);

        // 驗證輸入
        var validationResult = await ValidateInputAsync(request, cancellationToken);
        if (validationResult.IsFailure)
        {
            return Result<RegisterUserResponse>.Failure(validationResult.Error);
        }

        // 建立值物件
        try
        {
            var username = Username.Create(request.Username);
            var email = Email.Create(request.Email);

            // 取得公司網域設定
            var companyDomains = _configuration.GetSection("CompanySettings:Domains").Get<string[]>() ?? [];

            // 建立使用者
            var user = User.Create(username, email, request.FirstName, request.LastName, companyDomains);

            // 設定密碼
            var hashedPassword = _passwordHasher.HashPassword(request.Password);
            user.SetPassword(hashedPassword);

            // 儲存使用者
            await _userRepository.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User registered successfully with ID: {UserId}", user.Id.Value);

            // 建立回應
            var response = new RegisterUserResponse(
                user.Id.Value,
                user.Username.Value,
                user.Email.Value,
                user.Role.Type.ToString(),
                user.Status.ToString(),
                user.Status == UserStatus.PendingApproval);

            return Result<RegisterUserResponse>.Success(response);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Invalid input during user registration: {Message}", ex.Message);
            return Result<RegisterUserResponse>.Failure(new Error("USER.INVALID_INPUT", ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during user registration");
            return Result<RegisterUserResponse>.Failure(new Error("USER.REGISTRATION_FAILED", "註冊失敗，請稍後再試"));
        }
    }

    private async Task<Result> ValidateInputAsync(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // 檢查密碼是否一致
        if (request.Password != request.ConfirmPassword)
        {
            return Result.Failure(new Error("USER.PASSWORD_MISMATCH", "密碼確認不一致"));
        }

        // 檢查使用者名稱是否已存在
        var username = Username.Create(request.Username);
        if (await _userRepository.IsUsernameExistsAsync(username, cancellationToken))
        {
            return Result.Failure(new Error("USER.DUPLICATE_USERNAME", "使用者名稱已存在"));
        }

        // 檢查電子信箱是否已存在
        var email = Email.Create(request.Email);
        if (await _userRepository.IsEmailExistsAsync(email, cancellationToken))
        {
            return Result.Failure(new Error("USER.DUPLICATE_EMAIL", "電子信箱已被註冊"));
        }

        return Result.Success();
    }
}
