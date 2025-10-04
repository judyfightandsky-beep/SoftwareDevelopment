using MediatR;
using SoftwareDevelopment.Domain.Tasks.Repositories;
using SoftwareDevelopment.Domain.Tasks.ValueObjects;

namespace SoftwareDevelopment.Application.Tasks.Queries.GetTask;

/// <summary>
/// 取得任務查詢處理器
/// </summary>
public sealed class GetTaskQueryHandler : IRequestHandler<GetTaskQuery, TaskDto?>
{
    private readonly ITaskRepository _taskRepository;

    public GetTaskQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
    }

    /// <summary>
    /// 處理取得任務查詢
    /// </summary>
    /// <param name="request">查詢請求</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>任務 DTO</returns>
    public async Task<TaskDto?> Handle(GetTaskQuery request, CancellationToken cancellationToken)
    {
        var taskId = TaskId.From(request.TaskId);
        var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken);

        if (task == null)
        {
            return null;
        }

        return new TaskDto
        {
            Id = task.Id.Value,
            Title = task.Title,
            Description = task.Description,
            Type = task.Type.ToString(),
            Status = task.Status.ToString(),
            Priority = task.Priority.ToString(),
            ProjectId = task.ProjectId.Value,
            CreatedBy = task.CreatedBy.Value,
            AssignedTo = task.AssignedTo?.Value,
            EstimatedHours = task.EstimatedHours.Value,
            ActualHours = task.ActualHours.Value,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            LastModifiedAt = task.LastModifiedAt,
            StartedAt = task.StartedAt,
            CompletedAt = task.CompletedAt
        };
    }
}