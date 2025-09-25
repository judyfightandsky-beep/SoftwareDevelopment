namespace SoftwareDevelopment.Domain.Common;

/// <summary>
/// 儲存庫基底介面
/// 定義所有聚合根儲存庫的共通契約
/// </summary>
/// <typeparam name="TAggregate">聚合類型</typeparam>
/// <typeparam name="TId">識別碼類型</typeparam>
public interface IRepository<TAggregate, in TId>
    where TAggregate : class, IAggregateRoot
    where TId : class
{
    /// <summary>
    /// 根據識別碼取得聚合
    /// </summary>
    /// <param name="id">聚合識別碼</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>聚合實體，若不存在則返回 null</returns>
    Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 新增聚合
    /// </summary>
    /// <param name="aggregate">聚合實體</param>
    /// <param name="cancellationToken">取消權杖</param>
    Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新聚合
    /// </summary>
    /// <param name="aggregate">聚合實體</param>
    void Update(TAggregate aggregate);

    /// <summary>
    /// 移除聚合
    /// </summary>
    /// <param name="aggregate">聚合實體</param>
    void Remove(TAggregate aggregate);
}

/// <summary>
/// 聚合根標記介面
/// 標記實體為聚合根
/// </summary>
public interface IAggregateRoot
{
}
