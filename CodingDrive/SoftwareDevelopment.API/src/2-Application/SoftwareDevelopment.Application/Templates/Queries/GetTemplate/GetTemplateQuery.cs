using MediatR;

namespace SoftwareDevelopment.Application.Templates.Queries.GetTemplate;

/// <summary>
/// 取得模板查詢
/// </summary>
public sealed record GetTemplateQuery : IRequest<TemplateDto?>
{
    /// <summary>
    /// 模板 ID
    /// </summary>
    public Guid TemplateId { get; init; }
}