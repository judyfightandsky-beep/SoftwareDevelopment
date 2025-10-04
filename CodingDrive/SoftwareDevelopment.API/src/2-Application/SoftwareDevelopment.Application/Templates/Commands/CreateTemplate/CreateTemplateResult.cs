namespace SoftwareDevelopment.Application.Templates.Commands.CreateTemplate;

/// <summary>
/// 創建模板結果
/// </summary>
public sealed record CreateTemplateResult
{
    /// <summary>
    /// 模板 ID
    /// </summary>
    public Guid TemplateId { get; init; }

    /// <summary>
    /// 模板名稱
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool IsSuccess { get; init; }

    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public string? ErrorMessage { get; init; }
}