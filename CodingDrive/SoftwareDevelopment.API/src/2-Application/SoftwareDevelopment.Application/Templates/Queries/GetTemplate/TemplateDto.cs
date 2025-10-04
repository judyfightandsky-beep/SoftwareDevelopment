namespace SoftwareDevelopment.Application.Templates.Queries.GetTemplate;

/// <summary>
/// 模板資料傳輸物件
/// </summary>
public sealed record TemplateDto
{
    /// <summary>
    /// 模板 ID
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 模板名稱
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// 模板描述
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// 模板類型
    /// </summary>
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// 模板狀態
    /// </summary>
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// 創建者 ID
    /// </summary>
    public Guid CreatedBy { get; init; }

    /// <summary>
    /// 配置 JSON
    /// </summary>
    public string? ConfigurationJson { get; init; }

    /// <summary>
    /// 版本號
    /// </summary>
    public string Version { get; init; } = string.Empty;

    /// <summary>
    /// 標籤
    /// </summary>
    public string[] Tags { get; init; } = Array.Empty<string>();

    /// <summary>
    /// 下載次數
    /// </summary>
    public int DownloadCount { get; init; }

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// 最後修改時間
    /// </summary>
    public DateTime LastModifiedAt { get; init; }
}