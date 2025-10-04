using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoftwareDevelopment.Application.Users.Register;
using SoftwareDevelopment.Application.Users.Login;
using SoftwareDevelopment.Application.Users.Queries.GetUser;
using SoftwareDevelopment.Application.Users.Queries.GetUsers;
using SoftwareDevelopment.Api.DTOs.Users;
using SoftwareDevelopment.Api.DTOs.Common;

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
        if (request == null)
        {
            _logger.LogWarning("Register request is null");
            return BadRequest(new ErrorResponse
            {
                Error = "INVALID_REQUEST",
                Message = "請求內容不能為空"
            });
        }

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

            var errorResponse = new ErrorResponse
            {
                Error = result.Error.Code,
                Message = result.Error.Message
            };

            return result.Error.Code switch
            {
                "USER.DUPLICATE_USERNAME" or "USER.DUPLICATE_EMAIL" => Conflict(errorResponse),
                _ => BadRequest(errorResponse)
            };
        }

        _logger.LogInformation("User registered successfully with ID: {UserId}", result.Value!.UserId);

        return Created($"/api/users/{result.Value.UserId}", result.Value);
    }

    /// <summary>
    /// 使用者登入
    /// </summary>
    /// <param name="request">登入請求</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>登入結果</returns>
    /// <response code="200">登入成功</response>
    /// <response code="400">請求參數無效</response>
    /// <response code="401">驗證失敗</response>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginAsync(
        [FromBody] LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        if (request == null)
        {
            _logger.LogWarning("Login request is null");
            return BadRequest(new ErrorResponse
            {
                Error = "INVALID_REQUEST",
                Message = "請求內容不能為空"
            });
        }

        _logger.LogInformation("Processing user login for email: {Email}", request.Email);

        var command = new LoginUserCommand(
            request.Email,
            request.Password);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning("User login failed: {Error}", result.Error.Message);

            var errorResponse = new ErrorResponse
            {
                Error = result.Error.Code,
                Message = result.Error.Message
            };

            return result.Error.Code switch
            {
                "USER.INVALID_CREDENTIALS" => Unauthorized(errorResponse),
                _ => BadRequest(errorResponse)
            };
        }

        _logger.LogInformation("User logged in successfully: {Email}", request.Email);

        return Ok(result.Value);
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
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving user with ID: {UserId}", id);

        var query = new GetUserQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning("User not found: {UserId}", id);

            var errorResponse = new ErrorResponse
            {
                Error = result.Error.Code,
                Message = result.Error.Message
            };

            return NotFound(errorResponse);
        }

        var userResponse = new UserResponse
        {
            Id = result.Value!.Id,
            Username = result.Value.Username,
            Email = result.Value.Email,
            FirstName = result.Value.FirstName,
            LastName = result.Value.LastName,
            Role = result.Value.Role,
            IsEmailVerified = result.Value.IsEmailVerified,
            CreatedAt = result.Value.CreatedAt,
            LastLoginAt = result.Value.LastLoginAt
        };

        return Ok(userResponse);
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
    [ProducesResponseType(typeof(PagedUsersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUsersAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? role = null,
        [FromQuery] string? status = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving users list - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

        var query = new GetUsersQuery(pageNumber, pageSize, role, status);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning("Get users failed: {Error}", result.Error.Message);

            var errorResponse = new ErrorResponse
            {
                Error = result.Error.Code,
                Message = result.Error.Message
            };

            return BadRequest(errorResponse);
        }

        var users = result.Value!.Users.Select(u => new UserResponse
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Role = u.Role,
            IsEmailVerified = u.IsEmailVerified,
            CreatedAt = u.CreatedAt,
            LastLoginAt = u.LastLoginAt
        }).ToList();

        var response = new PagedUsersResponse
        {
            Data = users,
            Pagination = new PaginationMetadata
            {
                PageNumber = result.Value.PageNumber,
                PageSize = result.Value.PageSize,
                TotalCount = result.Value.TotalCount,
                TotalPages = result.Value.TotalPages,
                HasPreviousPage = result.Value.HasPreviousPage,
                HasNextPage = result.Value.HasNextPage
            }
        };

        return Ok(response);
    }
}
