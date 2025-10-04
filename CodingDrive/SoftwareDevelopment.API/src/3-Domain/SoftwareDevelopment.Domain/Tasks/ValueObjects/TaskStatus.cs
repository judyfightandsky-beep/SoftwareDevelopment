namespace SoftwareDevelopment.Domain.Tasks.ValueObjects;

/// <summary>
/// 任務狀態
/// </summary>
public enum TaskStatus
{
    /// <summary>
    /// 待辦
    /// </summary>
    Todo = 1,

    /// <summary>
    /// 進行中
    /// </summary>
    InProgress = 2,

    /// <summary>
    /// 等待審核
    /// </summary>
    InReview = 3,

    /// <summary>
    /// 等待測試
    /// </summary>
    InTesting = 4,

    /// <summary>
    /// 已完成
    /// </summary>
    Done = 5,

    /// <summary>
    /// 已取消
    /// </summary>
    Cancelled = 6,

    /// <summary>
    /// 已封存
    /// </summary>
    Archived = 7
}