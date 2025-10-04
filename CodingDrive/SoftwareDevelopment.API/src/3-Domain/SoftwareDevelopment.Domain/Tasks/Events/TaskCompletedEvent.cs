using SoftwareDevelopment.Domain.Common;
using SoftwareDevelopment.Domain.Tasks.ValueObjects;
using SoftwareDevelopment.Domain.Users.ValueObjects;

namespace SoftwareDevelopment.Domain.Tasks.Events;

/// <summary>
/// 任務完成事件
/// </summary>
public sealed record TaskCompletedEvent(
    TaskId TaskId,
    string Title,
    UserId CompletedBy,
    ActualHours ActualHours,
    EstimatedHours EstimatedHours,
    DateTime CompletedAt,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}