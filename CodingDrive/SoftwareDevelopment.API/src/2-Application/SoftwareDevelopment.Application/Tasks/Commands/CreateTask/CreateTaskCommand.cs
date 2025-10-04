using MediatR;
using SoftwareDevelopment.Application.Common;

namespace SoftwareDevelopment.Application.Tasks.Commands.CreateTask;

public sealed record CreateTaskCommand : IRequest<Result<CreateTaskResult>>
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Priority { get; init; } = string.Empty;
    public Guid ProjectId { get; init; }
    public Guid CreatedBy { get; init; }
    public Guid? AssignedTo { get; init; }
    public DateTime? DueDate { get; init; }
    public decimal? EstimatedHours { get; init; }
}