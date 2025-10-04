namespace SoftwareDevelopment.Domain.Users.ValueObjects;

/// <summary>
/// 使用者角色類型列舉
/// </summary>
public enum UserRoleType
{
    /// <summary>
    /// 訪客
    /// </summary>
    Guest = 0,

    /// <summary>
    /// 員工
    /// </summary>
    Employee = 1,

    /// <summary>
    /// 主管
    /// </summary>
    Manager = 2,

    /// <summary>
    /// 系統管理員
    /// </summary>
    SystemAdmin = 3
}
