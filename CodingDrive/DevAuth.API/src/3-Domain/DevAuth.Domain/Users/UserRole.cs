using DevAuth.Domain.Common;

namespace DevAuth.Domain.Users;

/// <summary>
/// 使用者角色值物件
/// </summary>
public sealed class UserRole : ValueObject
{
    private UserRole(UserRoleType type, DateTime assignedAt, UserId? assignedBy)
    {
        Type = type;
        AssignedAt = assignedAt;
        AssignedBy = assignedBy;
    }

    /// <summary>
    /// 角色類型
    /// </summary>
    public UserRoleType Type { get; }

    /// <summary>
    /// 指派時間
    /// </summary>
    public DateTime AssignedAt { get; }

    /// <summary>
    /// 指派者識別碼
    /// </summary>
    public UserId? AssignedBy { get; }

    /// <summary>
    /// 建立訪客角色
    /// </summary>
    /// <returns>訪客角色</returns>
    public static UserRole Guest() => new(UserRoleType.Guest, DateTime.UtcNow, null);

    /// <summary>
    /// 建立員工角色
    /// </summary>
    /// <param name="assignedBy">指派者</param>
    /// <returns>員工角色</returns>
    public static UserRole Employee(UserId assignedBy) =>
        new(UserRoleType.Employee, DateTime.UtcNow, assignedBy);

    /// <summary>
    /// 建立主管角色
    /// </summary>
    /// <param name="assignedBy">指派者</param>
    /// <returns>主管角色</returns>
    public static UserRole Manager(UserId assignedBy) =>
        new(UserRoleType.Manager, DateTime.UtcNow, assignedBy);

    /// <summary>
    /// 建立系統管理員角色
    /// </summary>
    /// <param name="assignedBy">指派者</param>
    /// <returns>系統管理員角色</returns>
    public static UserRole SystemAdmin(UserId assignedBy) =>
        new(UserRoleType.SystemAdmin, DateTime.UtcNow, assignedBy);

    /// <summary>
    /// 檢查是否具有指定權限
    /// </summary>
    /// <param name="permission">權限</param>
    /// <returns>是否具有權限</returns>
    public bool HasPermission(Permission permission)
    {
        return Type switch
        {
            UserRoleType.Guest => GuestPermissions.Contains(permission),
            UserRoleType.Employee => EmployeePermissions.Contains(permission),
            UserRoleType.Manager => ManagerPermissions.Contains(permission),
            UserRoleType.SystemAdmin => true, // 系統管理員具有所有權限
            _ => false
        };
    }

    /// <summary>
    /// 取得用於相等性比較的原子值
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Type;
        yield return AssignedAt;
        yield return AssignedBy;
    }

    private static readonly HashSet<Permission> GuestPermissions =
    [
        Permission.ViewPublicProjects,
        Permission.ViewPublicDocuments,
        Permission.SubmitFeedback,
        Permission.UpdateOwnProfile
    ];

    private static readonly HashSet<Permission> EmployeePermissions =
    [
        .. GuestPermissions,
        Permission.ViewPrivateProjects,
        Permission.CreateProjects,
        Permission.EditAssignedProjects,
        Permission.JoinTeams,
        Permission.InviteToTeam,
        Permission.ViewTeamMembers,
        Permission.AccessDevelopmentTools
    ];

    private static readonly HashSet<Permission> ManagerPermissions =
    [
        .. EmployeePermissions,
        Permission.CreateTeams,
        Permission.ManageTeamMembers,
        Permission.ApproveProjectCreation,
        Permission.DeleteProjects,
        Permission.ViewAllProjects,
        Permission.ManageDirectReports,
        Permission.ApproveTimeOff,
        Permission.ViewTeamReports
    ];
}