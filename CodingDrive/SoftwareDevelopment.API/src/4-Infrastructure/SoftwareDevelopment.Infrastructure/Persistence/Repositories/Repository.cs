using Microsoft.EntityFrameworkCore;
using SoftwareDevelopment.Domain.Common;
using SoftwareDevelopment.Infrastructure.Persistence;

namespace SoftwareDevelopment.Infrastructure.Persistence.Repositories;

/// <summary>
/// 儲存庫基底實作
/// </summary>
/// <typeparam name="TAggregate">聚合類型</typeparam>
/// <typeparam name="TId">識別碼類型</typeparam>
public abstract class Repository<TAggregate, TId> : IRepository<TAggregate, TId>
    where TAggregate : class, IAggregateRoot
    where TId : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TAggregate> _dbSet;

    protected Repository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<TAggregate>();
    }

    public virtual async Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        if (id == null)
            return null;

        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public virtual async Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
    {
        if (aggregate == null)
            throw new ArgumentNullException(nameof(aggregate));

        await _dbSet.AddAsync(aggregate, cancellationToken);
    }

    public virtual void Update(TAggregate aggregate)
    {
        if (aggregate == null)
            throw new ArgumentNullException(nameof(aggregate));

        _dbSet.Update(aggregate);
    }

    public virtual void Remove(TAggregate aggregate)
    {
        if (aggregate == null)
            throw new ArgumentNullException(nameof(aggregate));

        _dbSet.Remove(aggregate);
    }

    /// <summary>
    /// 更新聚合並保存變更
    /// </summary>
    public virtual async Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
    {
        Update(aggregate);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// 取得所有聚合
    /// </summary>
    public virtual async Task<IEnumerable<TAggregate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 分頁查詢
    /// </summary>
    public virtual async Task<(IEnumerable<TAggregate> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await _dbSet.CountAsync(cancellationToken);
        var items = await _dbSet
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    /// <summary>
    /// 檢查聚合是否存在
    /// </summary>
    public virtual async Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default)
    {
        if (id == null)
            return false;

        var aggregate = await GetByIdAsync(id, cancellationToken);
        return aggregate != null;
    }

    /// <summary>
    /// 計算聚合總數
    /// </summary>
    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(cancellationToken);
    }
}