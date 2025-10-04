namespace SoftwareDevelopment.Domain.Tasks.ValueObjects;

/// <summary>
/// 任務類型
/// </summary>
public enum TaskType
{
    /// <summary>
    /// 功能開發
    /// </summary>
    Feature = 1,

    /// <summary>
    /// 缺陷修復
    /// </summary>
    Bug = 2,

    /// <summary>
    /// 改進優化
    /// </summary>
    Enhancement = 3,

    /// <summary>
    /// 測試任務
    /// </summary>
    Testing = 4,

    /// <summary>
    /// 文檔撰寫
    /// </summary>
    Documentation = 5,

    /// <summary>
    /// 研究調查
    /// </summary>
    Research = 6,

    /// <summary>
    /// 重構
    /// </summary>
    Refactoring = 7,

    /// <summary>
    /// 維護
    /// </summary>
    Maintenance = 8,

    /// <summary>
    /// 其他
    /// </summary>
    Other = 99
}