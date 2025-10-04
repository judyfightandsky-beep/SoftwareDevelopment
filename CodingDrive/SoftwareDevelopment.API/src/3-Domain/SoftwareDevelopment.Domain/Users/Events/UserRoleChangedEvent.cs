using SoftwareDevelopment.Domain.Common;
using SoftwareDevelopment.Domain.Users.ValueObjects;

namespace SoftwareDevelopment.Domain.Users.Events;

/// <summary>
/// 使用者角色變更事件
/// </summary>
public sealed record UserRoleChangedEvent(
    UserId UserId,
    UserRoleType OldRole,
    UserRoleType NewRole,
    UserId ChangedBy,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}
