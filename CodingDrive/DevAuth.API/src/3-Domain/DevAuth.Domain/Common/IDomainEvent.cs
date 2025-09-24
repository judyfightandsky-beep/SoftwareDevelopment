namespace DevAuth.Domain.Common;

/// <summary>
/// 領域事件介面
/// 標記所有領域事件必須實作的契約
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// 事件識別碼
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// 事件發生時間
    /// </summary>
    DateTime OccurredOn { get; }
}