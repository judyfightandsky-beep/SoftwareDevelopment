namespace SoftwareDevelopment.Application.Tasks.Commands.CreateTask;

public sealed record CreateTaskResult
{
    public Guid TaskId { get; init; }
    public string Title { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
}