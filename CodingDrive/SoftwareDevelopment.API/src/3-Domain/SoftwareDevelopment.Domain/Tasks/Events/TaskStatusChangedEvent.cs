using SoftwareDevelopment.Domain.Common;
using SoftwareDevelopment.Domain.Tasks.ValueObjects;
using SoftwareDevelopment.Domain.Users.ValueObjects;

namespace SoftwareDevelopment.Domain.Tasks.Events;

/// <summary>
/// 任務狀態變更事件
/// </summary>
public sealed record TaskStatusChangedEvent(
    TaskId TaskId,
    SoftwareDevelopment.Domain.Tasks.ValueObjects.TaskStatus OldStatus,
    SoftwareDevelopment.Domain.Tasks.ValueObjects.TaskStatus NewStatus,
    UserId ChangedBy,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}