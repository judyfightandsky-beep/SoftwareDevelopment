namespace SoftwareDevelopment.Api.DTOs.Tasks;

/// <summary>
/// 創建任務請求
/// </summary>
public record CreateTaskRequest
{
    /// <summary>
    /// 任務標題
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// 任務描述
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// 任務類型
    /// </summary>
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// 優先級（1-5）
    /// </summary>
    public int Priority { get; init; } = 3;

    /// <summary>
    /// 專案 ID
    /// </summary>
    public Guid ProjectId { get; init; }

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
}