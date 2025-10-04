namespace SoftwareDevelopment.Application.Tasks.Queries.GetTask;

public sealed record TaskDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string Priority { get; init; } = string.Empty;
    public Guid ProjectId { get; init; }
    public Guid CreatedBy { get; init; }
    public Guid? AssignedTo { get; init; }
    public decimal EstimatedHours { get; init; }
    public decimal ActualHours { get; init; }
    public DateTime? DueDate { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastModifiedAt { get; init; }
    public DateTime? StartedAt { get; init; }
    public DateTime? CompletedAt { get; init; }
}