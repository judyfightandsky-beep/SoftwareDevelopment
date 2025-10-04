using MediatR;
using SoftwareDevelopment.Domain.Tasks.ValueObjects;
using SoftwareDevelopment.Domain.Users.ValueObjects;
using TaskStatusEnum = SoftwareDevelopment.Domain.Tasks.ValueObjects.TaskStatus;

namespace SoftwareDevelopment.Application.Tasks.Commands.UpdateTask;

public record UpdateTaskCommand : IRequest<UpdateTaskResult>
{
    public Guid TaskId { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public TaskType? Type { get; init; }
    public Priority? Priority { get; init; }
    public TaskStatusEnum? Status { get; init; }
    public Guid? AssignedTo { get; init; }
    public DateTime? DueDate { get; init; }
    public decimal? EstimatedHours { get; init; }
    public decimal? ActualHours { get; init; }
    public int? ProgressPercentage { get; init; }
    public Guid UpdatedBy { get; init; }
}