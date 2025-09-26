using MediatR;
using SoftwareDevelopment.Application.Common;
using SoftwareDevelopment.Domain.Shared;

namespace SoftwareDevelopment.Application.Users.Login;

/// <summary>
/// 使用者登入命令
/// </summary>
public sealed record LoginUserCommand(
    string Email,
    string Password
) : IRequest<Result<LoginUserResponse>>;

/// <summary>
/// 使用者登入回應
/// </summary>
public sealed record LoginUserResponse(
    Guid UserId,
    string Username,
    string Email,
    string AccessToken,
    string RefreshToken
);