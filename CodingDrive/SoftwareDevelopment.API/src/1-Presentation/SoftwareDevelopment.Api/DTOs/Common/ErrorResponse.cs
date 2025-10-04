namespace SoftwareDevelopment.Api.DTOs.Common;

public sealed record ErrorResponse
{
    public string Error { get; init; } = default!;
    public string Message { get; init; } = default!;
    public string? Details { get; init; }
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}