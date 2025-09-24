using DevAuth.Application.Common;

namespace DevAuth.Application.Users.Register;

/// <summary>
/// 註冊使用者命令
/// </summary>
/// <param name="Username">使用者名稱</param>
/// <param name="Email">電子信箱</param>
/// <param name="FirstName">名字</param>
/// <param name="LastName">姓氏</param>
/// <param name="Password">密碼</param>
/// <param name="ConfirmPassword">確認密碼</param>
public sealed record RegisterUserCommand(
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string Password,
    string ConfirmPassword) : ICommand<Result<RegisterUserResponse>>;