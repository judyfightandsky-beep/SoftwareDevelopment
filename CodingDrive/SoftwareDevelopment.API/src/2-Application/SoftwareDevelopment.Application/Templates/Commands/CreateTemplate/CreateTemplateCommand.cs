using MediatR;
using SoftwareDevelopment.Application.Common;

namespace SoftwareDevelopment.Application.Templates.Commands.CreateTemplate;

/// <summary>
/// 創建模板命令
/// </summary>
public sealed record CreateTemplateCommand : IRequest<Result<CreateTemplateResult>>
{
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
    /// 創建者 ID
    /// </summary>
    public Guid CreatedBy { get; init; }

    /// <summary>
    /// 配置 JSON
    /// </summary>
    public string? ConfigurationJson { get; init; }

    /// <summary>
    /// 標籤
    /// </summary>
    public string[] Tags { get; init; } = Array.Empty<string>();

    /// <summary>
    /// 版本號
    /// </summary>
    public string Version { get; init; } = "1.0.0";
}