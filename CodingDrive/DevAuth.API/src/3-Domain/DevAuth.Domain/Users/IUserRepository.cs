using DevAuth.Domain.Common;
using DevAuth.Domain.Shared;

namespace DevAuth.Domain.Users;

/// <summary>
/// 使用者儲存庫介面
/// 定義使用者聚合的持久化契約
/// </summary>
public interface IUserRepository : IRepository<User, UserId>
{
    /// <summary>
    /// 根據使用者名稱取得使用者
    /// </summary>
    /// <param name="username">使用者名稱</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>使用者實體，若不存在則返回 null</returns>
    Task<User?> GetByUsernameAsync(Username username, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根據電子信箱取得使用者
    /// </summary>
    /// <param name="email">電子信箱</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>使用者實體，若不存在則返回 null</returns>
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);

    /// <summary>
    /// 檢查使用者名稱是否已存在
    /// </summary>
    /// <param name="username">使用者名稱</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>是否已存在</returns>
    Task<bool> IsUsernameExistsAsync(Username username, CancellationToken cancellationToken = default);

    /// <summary>
    /// 檢查電子信箱是否已存在
    /// </summary>
    /// <param name="email">電子信箱</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>是否已存在</returns>
    Task<bool> IsEmailExistsAsync(Email email, CancellationToken cancellationToken = default);

    /// <summary>
    /// 取得待審核的使用者清單
    /// </summary>
    /// <param name="pageNumber">頁數</param>
    /// <param name="pageSize">每頁大小</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>待審核使用者清單</returns>
    Task<IEnumerable<User>> GetPendingApprovalUsersAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 根據角色取得使用者清單
    /// </summary>
    /// <param name="roleType">角色類型</param>
    /// <param name="pageNumber">頁數</param>
    /// <param name="pageSize">每頁大小</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>使用者清單</returns>
    Task<IEnumerable<User>> GetUsersByRoleAsync(
        UserRoleType roleType,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 取得使用者總數
    /// </summary>
    /// <param name="status">使用者狀態篩選（可選）</param>
    /// <param name="roleType">角色類型篩選（可選）</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>使用者總數</returns>
    Task<int> GetUserCountAsync(
        UserStatus? status = null,
        UserRoleType? roleType = null,
        CancellationToken cancellationToken = default);
}