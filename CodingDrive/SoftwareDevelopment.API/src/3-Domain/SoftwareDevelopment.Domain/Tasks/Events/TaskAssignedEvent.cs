using SoftwareDevelopment.Domain.Common;
using SoftwareDevelopment.Domain.Tasks.ValueObjects;
using SoftwareDevelopment.Domain.Users.ValueObjects;

namespace SoftwareDevelopment.Domain.Tasks.Events;

/// <summary>
/// 任務指派事件
/// </summary>
public sealed record TaskAssignedEvent(
    TaskId TaskId,
    UserId? PreviousAssignee,
    UserId NewAssignee,
    UserId AssignedBy,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}