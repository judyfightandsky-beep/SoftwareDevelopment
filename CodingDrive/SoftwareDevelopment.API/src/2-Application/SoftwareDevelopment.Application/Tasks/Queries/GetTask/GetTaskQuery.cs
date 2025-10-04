using MediatR;

namespace SoftwareDevelopment.Application.Tasks.Queries.GetTask;

public sealed record GetTaskQuery : IRequest<TaskDto?>
{
    public Guid TaskId { get; init; }
}