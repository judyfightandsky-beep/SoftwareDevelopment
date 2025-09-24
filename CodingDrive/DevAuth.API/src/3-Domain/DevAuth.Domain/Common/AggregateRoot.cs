namespace DevAuth.Domain.Common;

/// <summary>
/// 聚合根基底類別
/// 提供領域事件管理和基本識別功能
/// </summary>
/// <typeparam name="TId">聚合根識別碼類型</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>
    where TId : class
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected AggregateRoot(TId id) : base(id)
    {
    }

    /// <summary>
    /// 取得領域事件清單
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// 新增領域事件
    /// </summary>
    /// <param name="domainEvent">領域事件</param>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// 移除領域事件
    /// </summary>
    /// <param name="domainEvent">領域事件</param>
    protected void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        _domainEvents.Remove(domainEvent);
    }

    /// <summary>
    /// 清除所有領域事件
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}