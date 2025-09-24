using DevAuth.Domain.Common;

namespace DevAuth.Domain.Users.Events;

/// <summary>
/// 使用者啟用事件
/// </summary>
public sealed record UserActivatedEvent(
    UserId UserId,
    UserId ActivatedBy,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}