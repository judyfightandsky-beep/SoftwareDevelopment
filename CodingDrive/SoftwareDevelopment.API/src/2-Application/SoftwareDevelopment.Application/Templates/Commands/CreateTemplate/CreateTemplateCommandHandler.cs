using MediatR;
using SoftwareDevelopment.Application.Common;
using SoftwareDevelopment.Domain.Templates.Entities;
using SoftwareDevelopment.Domain.Templates.Repositories;
using SoftwareDevelopment.Domain.Templates.ValueObjects;
using SoftwareDevelopment.Domain.Users.ValueObjects;

namespace SoftwareDevelopment.Application.Templates.Commands.CreateTemplate;

/// <summary>
/// 創建模板命令處理器
/// </summary>
public sealed class CreateTemplateCommandHandler : IRequestHandler<CreateTemplateCommand, Result<CreateTemplateResult>>
{
    private readonly IProjectTemplateRepository _templateRepository;

    public CreateTemplateCommandHandler(IProjectTemplateRepository templateRepository)
    {
        _templateRepository = templateRepository ?? throw new ArgumentNullException(nameof(templateRepository));
    }

    public async Task<Result<CreateTemplateResult>> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // 解析模板類型
            if (!Enum.TryParse<TemplateType>(request.Type, true, out var templateType))
            {
                return Result<CreateTemplateResult>.Failure(
                    new Error("TEMPLATE.INVALID_TYPE", "無效的模板類型"));
            }

            // 創建配置
            var configuration = string.IsNullOrWhiteSpace(request.ConfigurationJson)
                ? TemplateConfiguration.Empty
                : TemplateConfiguration.Create(request.ConfigurationJson);

            // 創建元數據
            var metadata = TemplateMetadata.Create(request.Version, request.Tags);

            // 創建模板
            var template = ProjectTemplate.Create(
                request.Name,
                request.Description,
                templateType,
                UserId.From(request.CreatedBy),
                configuration,
                metadata);

            // 保存到資料庫
            await _templateRepository.AddAsync(template, cancellationToken);

            // 返回結果
            var result = new CreateTemplateResult
            {
                TemplateId = template.Id,
                Name = template.Name,
                CreatedAt = template.CreatedAt,
                IsSuccess = true
            };

            return Result<CreateTemplateResult>.Success(result);
        }
        catch (ArgumentException ex)
        {
            return Result<CreateTemplateResult>.Failure(
                new Error("TEMPLATE.VALIDATION_ERROR", ex.Message));
        }
        catch (Exception ex)
        {
            return Result<CreateTemplateResult>.Failure(
                new Error("TEMPLATE.CREATE_FAILED", $"創建模板失敗: {ex.Message}"));
        }
    }
}