using SoftwareDevelopment.Domain.Common;
using SoftwareDevelopment.Domain.Tasks.ValueObjects;
using SoftwareDevelopment.Domain.Users.ValueObjects;
using SoftwareDevelopment.Domain.Shared.ValueObjects;

namespace SoftwareDevelopment.Domain.Tasks.Events;

/// <summary>
/// 任務建立事件
/// </summary>
public sealed record TaskCreatedEvent(
    TaskId TaskId,
    string Title,
    TaskType Type,
    Priority Priority,
    ProjectId ProjectId,
    UserId CreatedBy,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}