using Microsoft.AspNetCore.Mvc;
using MediatR;
using SoftwareDevelopment.Application.Tasks.Commands.CreateTask;
using SoftwareDevelopment.Application.Tasks.Commands.UpdateTask;
using SoftwareDevelopment.Application.Tasks.Queries.GetTask;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using SoftwareDevelopment.Api.DTOs.Tasks;
using SoftwareDevelopment.Domain.Tasks.ValueObjects;
using TaskStatusEnum = SoftwareDevelopment.Domain.Tasks.ValueObjects.TaskStatus;

namespace SoftwareDevelopment.Api.Controllers;

/// <summary>
/// 任務管理控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TasksController> _logger;

    public TasksController(IMediator mediator, ILogger<TasksController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// 創建新任務
    /// </summary>
    /// <param name="request">創建任務請求</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>創建結果</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateTaskResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateTask(
        [FromBody] CreateTaskRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = GetCurrentUserId();
            var command = new CreateTaskCommand
            {
                Title = request.Title,
                Description = request.Description,
                Type = request.Type,
                Priority = request.Priority.ToString(),
                ProjectId = request.ProjectId,
                CreatedBy = userId,
                AssignedTo = request.AssignedTo,
                DueDate = request.DueDate,
                EstimatedHours = request.EstimatedHours
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "創建任務失敗",
                    Detail = result.Error.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }

            return CreatedAtAction(
                nameof(GetTask),
                new { id = result.Value?.TaskId },
                result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "創建任務時發生錯誤");
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "內部服務器錯誤",
                Detail = "創建任務時發生未預期的錯誤",
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    /// <summary>
    /// 取得任務詳情
    /// </summary>
    /// <param name="id">任務 ID</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>任務詳情</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTask(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetTaskQuery { TaskId = id };
            var result = await _mediator.Send(query, cancellationToken);

            if (result == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "任務不存在",
                    Detail = $"找不到 ID 為 {id} 的任務",
                    Status = StatusCodes.Status404NotFound
                });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "取得任務時發生錯誤: {TaskId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "內部服務器錯誤",
                Detail = "取得任務時發生未預期的錯誤",
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    /// <summary>
    /// 更新任務
    /// </summary>
    /// <param name="id">任務 ID</param>
    /// <param name="request">更新任務請求</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>更新結果</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UpdateTaskResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateTask(
        [FromRoute] Guid id,
        [FromBody] UpdateTaskRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = GetCurrentUserId();
            var command = new UpdateTaskCommand
            {
                TaskId = id,
                Title = request.Title,
                Description = request.Description,
                Type = !string.IsNullOrEmpty(request.Type) ? Enum.Parse<TaskType>(request.Type) : null,
                Priority = request.Priority.HasValue ? (Priority)request.Priority.Value : null,
                Status = !string.IsNullOrEmpty(request.Status) ? Enum.Parse<TaskStatusEnum>(request.Status) : null,
                AssignedTo = request.AssignedTo,
                DueDate = request.DueDate,
                EstimatedHours = request.EstimatedHours,
                ActualHours = request.ActualHours,
                ProgressPercentage = request.ProgressPercentage,
                UpdatedBy = userId
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (!result.Success)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "更新任務失敗",
                    Detail = result.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "更新任務時發生錯誤: {TaskId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "內部服務器錯誤",
                Detail = "更新任務時發生未預期的錯誤",
                Status = StatusCodes.Status500InternalServerError
            });
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