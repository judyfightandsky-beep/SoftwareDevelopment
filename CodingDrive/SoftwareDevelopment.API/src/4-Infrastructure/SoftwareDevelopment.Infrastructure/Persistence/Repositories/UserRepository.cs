using Microsoft.EntityFrameworkCore;
using SoftwareDevelopment.Domain.Users;
using SoftwareDevelopment.Domain.Shared;

namespace SoftwareDevelopment.Infrastructure.Persistence.Repositories;

/// <summary>
/// 使用者儲存庫實作
/// </summary>
public sealed class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// 初始化使用者儲存庫
    /// </summary>
    /// <param name="context">資料庫內容</param>
    public UserRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// 根據識別碼取得使用者
    /// </summary>
    /// <param name="id">使用者識別碼</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>使用者實體，若不存在則返回 null</returns>
    public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    /// <summary>
    /// 根據使用者名稱取得使用者
    /// </summary>
    /// <param name="username">使用者名稱</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>使用者實體，若不存在則返回 null</returns>
    public async Task<User?> GetByUsernameAsync(Username username, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    /// <summary>
    /// 根據電子信箱取得使用者
    /// </summary>
    /// <param name="email">電子信箱</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>使用者實體，若不存在則返回 null</returns>
    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    /// <summary>
    /// 檢查使用者名稱是否已存在
    /// </summary>
    /// <param name="username">使用者名稱</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>是否已存在</returns>
    public async Task<bool> IsUsernameExistsAsync(Username username, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AnyAsync(u => u.Username == username, cancellationToken);
    }

    /// <summary>
    /// 檢查電子信箱是否已存在
    /// </summary>
    /// <param name="email">電子信箱</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>是否已存在</returns>
    public async Task<bool> IsEmailExistsAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email, cancellationToken);
    }

    /// <summary>
    /// 取得待審核的使用者清單
    /// </summary>
    /// <param name="pageNumber">頁數</param>
    /// <param name="pageSize">每頁大小</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>待審核使用者清單</returns>
    public async Task<IEnumerable<User>> GetPendingApprovalUsersAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.Status == UserStatus.PendingApproval)
            .OrderBy(u => u.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 根據角色取得使用者清單
    /// </summary>
    /// <param name="roleType">角色類型</param>
    /// <param name="pageNumber">頁數</param>
    /// <param name="pageSize">每頁大小</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>使用者清單</returns>
    public async Task<IEnumerable<User>> GetUsersByRoleAsync(
        UserRoleType roleType,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.Role.Type == roleType)
            .OrderBy(u => u.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 取得使用者總數
    /// </summary>
    /// <param name="status">使用者狀態篩選（可選）</param>
    /// <param name="roleType">角色類型篩選（可選）</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>使用者總數</returns>
    public async Task<int> GetUserCountAsync(
        UserStatus? status = null,
        UserRoleType? roleType = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Users.AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(u => u.Status == status.Value);
        }

        if (roleType.HasValue)
        {
            query = query.Where(u => u.Role.Type == roleType.Value);
        }

        return await query.CountAsync(cancellationToken);
    }

    /// <summary>
    /// 新增使用者
    /// </summary>
    /// <param name="aggregate">使用者實體</param>
    /// <param name="cancellationToken">取消權杖</param>
    public async Task AddAsync(User aggregate, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(aggregate, cancellationToken);
    }

    /// <summary>
    /// 更新使用者
    /// </summary>
    /// <param name="aggregate">使用者實體</param>
    public void Update(User aggregate)
    {
        _context.Users.Update(aggregate);
    }

    /// <summary>
    /// 移除使用者
    /// </summary>
    /// <param name="aggregate">使用者實體</param>
    public void Remove(User aggregate)
    {
        _context.Users.Remove(aggregate);
    }
}
