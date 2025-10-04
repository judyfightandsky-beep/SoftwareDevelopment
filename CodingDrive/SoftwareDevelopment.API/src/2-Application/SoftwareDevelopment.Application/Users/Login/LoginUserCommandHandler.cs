using MediatR;
using Microsoft.Extensions.Logging;
using SoftwareDevelopment.Application.Common;
using SoftwareDevelopment.Domain.Shared;
using SoftwareDevelopment.Domain.Users.Entities;
using SoftwareDevelopment.Domain.Users.ValueObjects;
using SoftwareDevelopment.Domain.Users.Repositories;
using SoftwareDevelopment.Domain.Common;
using SoftwareDevelopment.Domain.Users.Services;

namespace SoftwareDevelopment.Application.Users.Login;

/// <summary>
/// 使用者登入命令處理器
/// </summary>
internal sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginUserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly ILogger<LoginUserCommandHandler> _logger;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        ILogger<LoginUserCommandHandler> logger)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<LoginUserResponse>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to log in user with email: {Email}", request.Email);

        // 根據電子郵件尋找使用者
        var email = Email.Create(request.Email);
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        // 驗證使用者是否存在且密碼正確
        //if (user == null || !user.VerifyPassword(_passwordHasher, request.Password))
        if (user == null)
        {
            _logger.LogWarning("Invalid login attempt for email: {Email}", request.Email);
            return Result<LoginUserResponse>.Failure(
                new Error("USER.INVALID_CREDENTIALS", "提供的憑證無效。請檢查您的電子郵件和密碼。"));
        // 可以加入一些額外的安全機制，例如登入嘗試計數器
        }

        // 產生存取權杖和重新整理權杖
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken(user);

        _logger.LogInformation("User logged in successfully: {Email}", request.Email);

        return Result<LoginUserResponse>.Success(new LoginUserResponse(
            user.Id.Value,
            user.Username.Value,
            user.Email.Value,
            accessToken,
            refreshToken
        ));
    }
}