namespace SoftwareDevelopment.Domain.Users.ValueObjects;

/// <summary>
/// 系統權限列舉
/// </summary>
public enum Permission
{
    // 基本權限
    ViewPublicProjects,
    ViewPublicDocuments,
    SubmitFeedback,
    UpdateOwnProfile,

    // 員工權限
    ViewPrivateProjects,
    CreateProjects,
    EditAssignedProjects,
    JoinTeams,
    InviteToTeam,
    ViewTeamMembers,
    AccessDevelopmentTools,

    // 主管權限
    CreateTeams,
    ManageTeamMembers,
    ApproveProjectCreation,
    DeleteProjects,
    ViewAllProjects,
    ManageDirectReports,
    ApproveTimeOff,
    ViewTeamReports,

    // 系統管理員權限
    ManageAllUsers,
    ManageAllTeams,
    ManageAllProjects,
    SystemConfiguration,
    ViewAuditLogs,
    ManageRolesAndPermissions
}
