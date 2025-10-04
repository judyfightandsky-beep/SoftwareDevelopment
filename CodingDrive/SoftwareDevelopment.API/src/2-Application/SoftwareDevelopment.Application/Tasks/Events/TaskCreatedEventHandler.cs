using MediatR;
using Microsoft.Extensions.Logging;
using SoftwareDevelopment.Application.Common;
using SoftwareDevelopment.Domain.Tasks.Events;

namespace SoftwareDevelopment.Application.Tasks.Events;

/// <summary>
/// 任務建立事件處理器
/// 處理任務建立後的應用層業務邏輯
/// </summary>
public sealed class TaskCreatedEventHandler : INotificationHandler<DomainEventNotification<TaskCreatedEvent>>
{
    private readonly ILogger<TaskCreatedEventHandler> _logger;

    public TaskCreatedEventHandler(ILogger<TaskCreatedEventHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// 處理任務建立事件
    /// </summary>
    /// <param name="notification">領域事件通知包裝器</param>
    /// <param name="cancellationToken">取消權杖</param>
    public async Task Handle(DomainEventNotification<TaskCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var taskCreatedEvent = notification.DomainEvent;

        _logger.LogInformation(
            "處理任務建立事件: TaskId={TaskId}, Title={Title}, Type={Type}, Priority={Priority}",
            taskCreatedEvent.TaskId,
            taskCreatedEvent.Title,
            taskCreatedEvent.Type,
            taskCreatedEvent.Priority);

        // 這裡可以加入應用層的業務邏輯
        // 例如：更新專案統計、通知團隊成員等

        await Task.CompletedTask;
    }
}