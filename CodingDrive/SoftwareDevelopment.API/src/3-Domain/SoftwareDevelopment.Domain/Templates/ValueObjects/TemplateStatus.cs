namespace SoftwareDevelopment.Domain.Templates.ValueObjects;

/// <summary>
/// 專案模板狀態
/// </summary>
public enum TemplateStatus
{
    /// <summary>
    /// 草稿
    /// </summary>
    Draft = 1,

    /// <summary>
    /// 審核中
    /// </summary>
    UnderReview = 2,

    /// <summary>
    /// 已發布
    /// </summary>
    Published = 3,

    /// <summary>
    /// 已封存
    /// </summary>
    Archived = 4,

    /// <summary>
    /// 已刪除
    /// </summary>
    Deleted = 5
}