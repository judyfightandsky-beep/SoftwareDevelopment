using MediatR;
using SoftwareDevelopment.Domain.Templates.Repositories;
using SoftwareDevelopment.Domain.Templates.ValueObjects;

namespace SoftwareDevelopment.Application.Templates.Queries.GetTemplate;

/// <summary>
/// 取得模板查詢處理器
/// </summary>
public sealed class GetTemplateQueryHandler : IRequestHandler<GetTemplateQuery, TemplateDto?>
{
    private readonly IProjectTemplateRepository _templateRepository;

    public GetTemplateQueryHandler(IProjectTemplateRepository templateRepository)
    {
        _templateRepository = templateRepository ?? throw new ArgumentNullException(nameof(templateRepository));
    }

    public async Task<TemplateDto?> Handle(GetTemplateQuery request, CancellationToken cancellationToken)
    {
        var templateId = TemplateId.From(request.TemplateId);
        var template = await _templateRepository.GetByIdAsync(templateId, cancellationToken);

        if (template == null)
        {
            return null;
        }

        return new TemplateDto
        {
            Id = template.Id,
            Name = template.Name,
            Description = template.Description,
            Type = template.Type.ToString(),
            Status = template.Status.ToString(),
            CreatedBy = template.CreatedBy,
            ConfigurationJson = template.Configuration.ConfigurationJson,
            Version = template.Metadata.Version,
            Tags = template.Metadata.Tags.ToArray(),
            DownloadCount = template.DownloadCount,
            CreatedAt = template.CreatedAt,
            LastModifiedAt = template.LastModifiedAt
        };
    }
}