using DevAuth.Domain.Common;
using DevAuth.Domain.Shared;

namespace DevAuth.Domain.Users.Events;

/// <summary>
/// 使用者建立事件
/// </summary>
public sealed record UserCreatedEvent(
    UserId UserId,
    Email Email,
    UserRoleType RoleType,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}