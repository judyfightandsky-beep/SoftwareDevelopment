namespace SoftwareDevelopment.Api.DTOs.Tasks;

/// <summary>
/// 更新任務請求
/// </summary>
public record UpdateTaskRequest
{
    /// <summary>
    /// 任務標題
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// 任務描述
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// 任務類型
    /// </summary>
    public string? Type { get; init; }

    /// <summary>
    /// 優先級（1-5）
    /// </summary>
    public int? Priority { get; init; }

    /// <summary>
    /// 任務狀態
    /// </summary>
    public string? Status { get; init; }

    /// <summary>
    /// 指派給（可選）
    /// </summary>
    public Guid? AssignedTo { get; init; }

    /// <summary>
    /// 截止日期（可選）
    /// </summary>
    public DateTime? DueDate { get; init; }

    /// <summary>
    /// 估算工時（可選）
    /// </summary>
    public decimal? EstimatedHours { get; init; }

    /// <summary>
    /// 實際工時（可選）
    /// </summary>
    public decimal? ActualHours { get; init; }

    /// <summary>
    /// 進度百分比（0-100）
    /// </summary>
    public int? ProgressPercentage { get; init; }
}