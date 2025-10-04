using SoftwareDevelopment.Domain.Common;
using SoftwareDevelopment.Domain.Shared;
using SoftwareDevelopment.Domain.Users.ValueObjects;

namespace SoftwareDevelopment.Domain.Users.Events;

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
