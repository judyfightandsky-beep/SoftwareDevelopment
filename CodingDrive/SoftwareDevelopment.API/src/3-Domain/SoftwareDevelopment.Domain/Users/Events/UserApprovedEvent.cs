using SoftwareDevelopment.Domain.Common;
using SoftwareDevelopment.Domain.Users.ValueObjects;

namespace SoftwareDevelopment.Domain.Users.Events;

/// <summary>
/// 使用者核准事件
/// </summary>
public sealed record UserApprovedEvent(
    UserId UserId,
    UserId ApprovedBy,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}
