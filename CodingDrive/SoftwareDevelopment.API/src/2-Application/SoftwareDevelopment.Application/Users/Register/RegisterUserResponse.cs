namespace SoftwareDevelopment.Application.Users.Register;

/// <summary>
/// 註冊使用者回應
/// </summary>
/// <param name="UserId">使用者識別碼</param>
/// <param name="Username">使用者名稱</param>
/// <param name="Email">電子信箱</param>
/// <param name="Role">角色類型</param>
/// <param name="Status">使用者狀態</param>
/// <param name="RequiresApproval">是否需要審核</param>
public sealed record RegisterUserResponse(
    Guid UserId,
    string Username,
    string Email,
    string Role,
    string Status,
    bool RequiresApproval);
