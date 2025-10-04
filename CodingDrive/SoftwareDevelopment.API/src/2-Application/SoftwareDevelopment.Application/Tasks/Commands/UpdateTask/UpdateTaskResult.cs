namespace SoftwareDevelopment.Application.Tasks.Commands.UpdateTask;

public record UpdateTaskResult
{
    public Guid TaskId { get; init; }
    public bool Success { get; init; }
    public string? Message { get; init; }
}