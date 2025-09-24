using DevAuth.Domain.Common;

namespace DevAuth.Domain.Users.Events;

/// <summary>
/// 使用者停用事件
/// </summary>
public sealed record UserDeactivatedEvent(
    UserId UserId,
    UserId DeactivatedBy,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}