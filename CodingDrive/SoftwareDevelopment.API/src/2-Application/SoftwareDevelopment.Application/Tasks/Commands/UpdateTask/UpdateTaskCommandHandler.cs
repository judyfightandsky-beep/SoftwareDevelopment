using MediatR;
using SoftwareDevelopment.Domain.Tasks.Repositories;
using SoftwareDevelopment.Domain.Tasks.ValueObjects;
using SoftwareDevelopment.Domain.Users.ValueObjects;

namespace SoftwareDevelopment.Application.Tasks.Commands.UpdateTask;

public sealed class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, UpdateTaskResult>
{
    private readonly ITaskRepository _taskRepository;

    public UpdateTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
    }

    public async Task<UpdateTaskResult> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var taskId = TaskId.From(request.TaskId);
            var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken);

            if (task == null)
            {
                return new UpdateTaskResult
                {
                    TaskId = request.TaskId,
                    Success = false,
                    Message = "任務不存在"
                };
            }

            // 更新任務資訊
            if (!string.IsNullOrWhiteSpace(request.Title) && !string.IsNullOrWhiteSpace(request.Description))
            {
                var estimatedHours = request.EstimatedHours.HasValue
                    ? EstimatedHours.Create(request.EstimatedHours.Value)
                    : null;

                task.Update(
                    request.Title,
                    request.Description,
                    request.Type ?? task.Type,
                    request.Priority ?? task.Priority,
                    estimatedHours,
                    request.DueDate);
            }

            // 更新指派
            if (request.AssignedTo.HasValue)
            {
                var assignedTo = UserId.From(request.AssignedTo.Value);
                task.AssignTo(assignedTo);
            }

            // 更新狀態
            if (request.Status.HasValue)
            {
                task.UpdateStatus(request.Status.Value);
            }

            // 記錄工時
            if (request.ActualHours.HasValue && request.ActualHours.Value > 0)
            {
                var currentActualHours = task.ActualHours.Value;
                var additionalHours = request.ActualHours.Value - currentActualHours;
                if (additionalHours > 0)
                {
                    task.LogHours(additionalHours);
                }
            }

            await _taskRepository.UpdateAsync(task, cancellationToken);

            return new UpdateTaskResult
            {
                TaskId = request.TaskId,
                Success = true,
                Message = "任務更新成功"
            };
        }
        catch (Exception ex)
        {
            return new UpdateTaskResult
            {
                TaskId = request.TaskId,
                Success = false,
                Message = $"更新任務時發生錯誤: {ex.Message}"
            };
        }
    }
}