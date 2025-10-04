namespace SoftwareDevelopment.Domain.Tasks.ValueObjects;

/// <summary>
/// 任務優先級
/// </summary>
public enum Priority
{
    /// <summary>
    /// 最低
    /// </summary>
    Lowest = 1,

    /// <summary>
    /// 低
    /// </summary>
    Low = 2,

    /// <summary>
    /// 中等
    /// </summary>
    Medium = 3,

    /// <summary>
    /// 高
    /// </summary>
    High = 4,

    /// <summary>
    /// 最高
    /// </summary>
    Highest = 5,

    /// <summary>
    /// 緊急
    /// </summary>
    Critical = 6
}