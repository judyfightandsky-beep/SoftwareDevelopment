namespace SoftwareDevelopment.Domain.Users;

/// <summary>
/// 使用者狀態列舉
/// </summary>
public enum UserStatus
{
    /// <summary>
    /// 待驗證 - 使用者已註冊但尚未驗證電子信箱
    /// </summary>
    PendingVerification = 0,

    /// <summary>
    /// 待審核 - 員工角色等待管理員審核
    /// </summary>
    PendingApproval = 1,

    /// <summary>
    /// 啟用 - 帳號已啟用可正常使用
    /// </summary>
    Active = 2,

    /// <summary>
    /// 停用 - 帳號被暫時停用
    /// </summary>
    Inactive = 3,

    /// <summary>
    /// 封鎖 - 帳號被永久封鎖
    /// </summary>
    Blocked = 4
}
