using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoftwareDevelopment.Application.Users.Register;

namespace SoftwareDevelopment.Api.Controllers;

/// <summary>
/// 使用者管理控制器
/// 提供使用者註冊、查詢和管理功能
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersController> _logger;

    /// <summary>
    /// 初始化使用者控制器
    /// </summary>
    /// <param name="mediator">中介者</param>
    /// <param name="logger">日誌記錄器</param>
    public UsersController(IMediator mediator, ILogger<UsersController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// 註冊新使用者
    /// </summary>
    /// <param name="request">註冊請求</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>註冊結果</returns>
    /// <response code="201">使用者註冊成功</response>
    /// <response code="400">請求參數無效</response>
    /// <response code="409">使用者名稱或電子信箱已存在</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterAsync(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing user registration for email: {Email}", request.Email);

        var command = new RegisterUserCommand(
            request.Username,
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password,
            request.ConfirmPassword);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning("User registration failed: {Error}", result.Error.Message);

            return result.Error.Code switch
            {
                "USER.DUPLICATE_USERNAME" or "USER.DUPLICATE_EMAIL" => Conflict(new
                {
                    error = result.Error.Code,
                    message = result.Error.Message
                }),
                _ => BadRequest(new
                {
                    error = result.Error.Code,
                    message = result.Error.Message
                })
            };
        }

        _logger.LogInformation("User registered successfully with ID: {UserId}", result.Value!.UserId);

        return Created($"/api/users/{result.Value.UserId}", result.Value);
    }

    /// <summary>
    /// 根據識別碼取得使用者
    /// </summary>
    /// <param name="id">使用者識別碼</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>使用者資訊</returns>
    /// <response code="200">取得使用者成功</response>
    /// <response code="404">使用者不存在</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving user with ID: {UserId}", id);

        // TODO: 實作取得使用者查詢
        await Task.CompletedTask;

        return NotFound();
    }

    /// <summary>
    /// 取得使用者清單
    /// </summary>
    /// <param name="pageNumber">頁數（從 1 開始）</param>
    /// <param name="pageSize">每頁大小（預設 20，最大 100）</param>
    /// <param name="role">角色篩選</param>
    /// <param name="status">狀態篩選</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>使用者清單</returns>
    /// <response code="200">取得使用者清單成功</response>
    /// <response code="400">請求參數無效</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUsersAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? role = null,
        [FromQuery] string? status = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving users list - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

        if (pageNumber < 1)
        {
            return BadRequest("頁數必須大於 0");
        }

        if (pageSize is < 1 or > 100)
        {
            return BadRequest("每頁大小必須介於 1-100 之間");
        }

        // TODO: 實作取得使用者清單查詢
        await Task.CompletedTask;

        return Ok(new
        {
            data = Array.Empty<object>(),
            pagination = new
            {
                pageNumber,
                pageSize,
                totalCount = 0,
                totalPages = 0
            }
        });
    }
}

/// <summary>
/// 註冊使用者請求
/// </summary>
/// <param name="Username">使用者名稱</param>
/// <param name="Email">電子信箱</param>
/// <param name="FirstName">名字</param>
/// <param name="LastName">姓氏</param>
/// <param name="Password">密碼</param>
/// <param name="ConfirmPassword">確認密碼</param>
public sealed record RegisterUserRequest(
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string Password,
    string ConfirmPassword);
