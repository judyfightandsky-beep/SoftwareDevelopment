using Microsoft.AspNetCore.Mvc;
using MediatR;
using SoftwareDevelopment.Application.Templates.Commands.CreateTemplate;
using SoftwareDevelopment.Application.Templates.Queries.GetTemplate;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SoftwareDevelopment.Api.Controllers;

/// <summary>
/// 專案模板管理控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TemplatesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TemplatesController> _logger;

    public TemplatesController(IMediator mediator, ILogger<TemplatesController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// 創建新模板
    /// </summary>
    /// <param name="request">創建模板請求</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>創建結果</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateTemplateResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateTemplate(
        [FromBody] CreateTemplateRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = GetCurrentUserId();
            var command = new CreateTemplateCommand
            {
                Name = request.Name,
                Description = request.Description,
                Type = request.Type,
                CreatedBy = userId,
                ConfigurationJson = request.ConfigurationJson,
                Tags = request.Tags,
                Version = request.Version
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "創建模板失敗",
                    Detail = result.Error.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }

            return CreatedAtAction(
                nameof(GetTemplate),
                new { id = result.Value?.TemplateId },
                result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "創建模板時發生錯誤");
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "內部服務器錯誤",
                Detail = "創建模板時發生未預期的錯誤",
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    /// <summary>
    /// 取得模板詳情
    /// </summary>
    /// <param name="id">模板 ID</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>模板詳情</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TemplateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTemplate(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetTemplateQuery { TemplateId = id };
            var result = await _mediator.Send(query, cancellationToken);

            if (result == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "模板不存在",
                    Detail = $"找不到 ID 為 {id} 的模板",
                    Status = StatusCodes.Status404NotFound
                });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "取得模板時發生錯誤: {TemplateId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "內部服務器錯誤",
                Detail = "取得模板時發生未預期的錯誤",
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    /// <summary>
    /// 搜尋模板
    /// </summary>
    /// <param name="searchTerm">搜尋詞</param>
    /// <param name="type">模板類型</param>
    /// <param name="category">模板分類</param>
    /// <param name="tags">標籤</param>
    /// <param name="pageNumber">頁碼</param>
    /// <param name="pageSize">頁面大小</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>搜尋結果</returns>
    [HttpGet("search")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SearchTemplatesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> SearchTemplates(
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? type = null,
        [FromQuery] string? category = null,
        [FromQuery] string[]? tags = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // TODO: 實作搜尋查詢
            // var query = new SearchTemplatesQuery { ... };
            // var result = await _mediator.Send(query, cancellationToken);

            // 暫時回傳空結果
            var result = new SearchTemplatesResponse
            {
                Templates = Array.Empty<TemplateDto>(),
                TotalCount = 0,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = 0
            };

            return Task.FromResult<IActionResult>(Ok(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "搜尋模板時發生錯誤");
            return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "內部服務器錯誤",
                Detail = "搜尋模板時發生未預期的錯誤",
                Status = StatusCodes.Status500InternalServerError
            }));
        }
    }

    /// <summary>
    /// 取得熱門模板
    /// </summary>
    /// <param name="count">數量</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>熱門模板列表</returns>
    [HttpGet("popular")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<TemplateDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetPopularTemplates(
        [FromQuery] int count = 10,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // TODO: 實作熱門模板查詢
            // var query = new GetPopularTemplatesQuery { Count = count };
            // var result = await _mediator.Send(query, cancellationToken);

            // 暫時回傳空結果
            var result = Array.Empty<TemplateDto>();
            return Task.FromResult<IActionResult>(Ok(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "取得熱門模板時發生錯誤");
            return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "內部服務器錯誤",
                Detail = "取得熱門模板時發生未預期的錯誤",
                Status = StatusCodes.Status500InternalServerError
            }));
        }
    }

    /// <summary>
    /// 下載模板
    /// </summary>
    /// <param name="id">模板 ID</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>下載結果</returns>
    [HttpPost("{id:guid}/download")]
    [ProducesResponseType(typeof(DownloadTemplateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> DownloadTemplate(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // TODO: 實作模板下載邏輯
            // 1. 驗證模板存在且可下載
            // 2. 增加下載次數
            // 3. 回傳下載連結或直接回傳檔案

            var response = new DownloadTemplateResponse
            {
                TemplateId = id,
                DownloadUrl = $"/api/templates/{id}/files",
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            return Task.FromResult<IActionResult>(Ok(response));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "下載模板時發生錯誤: {TemplateId}", id);
            return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "內部服務器錯誤",
                Detail = "下載模板時發生未預期的錯誤",
                Status = StatusCodes.Status500InternalServerError
            }));
        }
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("無法取得當前用戶 ID");
        }
        return userId;
    }
}

/// <summary>
/// 創建模板請求
/// </summary>
public record CreateTemplateRequest
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
    /// 模板配置（JSON 格式）
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

/// <summary>
/// 搜尋模板回應
/// </summary>
public record SearchTemplatesResponse
{
    /// <summary>
    /// 模板列表
    /// </summary>
    public IEnumerable<TemplateDto> Templates { get; init; } = Array.Empty<TemplateDto>();

    /// <summary>
    /// 總數量
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// 當前頁碼
    /// </summary>
    public int PageNumber { get; init; }

    /// <summary>
    /// 頁面大小
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// 總頁數
    /// </summary>
    public int TotalPages { get; init; }
}

/// <summary>
/// 下載模板回應
/// </summary>
public record DownloadTemplateResponse
{
    /// <summary>
    /// 模板 ID
    /// </summary>
    public Guid TemplateId { get; init; }

    /// <summary>
    /// 下載 URL
    /// </summary>
    public string DownloadUrl { get; init; } = string.Empty;

    /// <summary>
    /// 過期時間
    /// </summary>
    public DateTime ExpiresAt { get; init; }
}