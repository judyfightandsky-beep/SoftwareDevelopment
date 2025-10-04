using MediatR;
using Microsoft.Extensions.Logging;
using SoftwareDevelopment.Application.Common;
using SoftwareDevelopment.Domain.Users.Events;

namespace SoftwareDevelopment.Application.Users.Events;

/// <summary>
/// 使用者建立事件處理器
/// 處理使用者建立後的應用層業務邏輯
/// </summary>
public sealed class UserCreatedEventHandler : INotificationHandler<DomainEventNotification<UserCreatedEvent>>
{
    private readonly ILogger<UserCreatedEventHandler> _logger;

    public UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// 處理使用者建立事件
    /// </summary>
    /// <param name="notification">領域事件通知包裝器</param>
    /// <param name="cancellationToken">取消權杖</param>
    public async Task Handle(DomainEventNotification<UserCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var userCreatedEvent = notification.DomainEvent;

        _logger.LogInformation(
            "處理使用者建立事件: UserId={UserId}, Email={Email}, RoleType={RoleType}",
            userCreatedEvent.UserId,
            userCreatedEvent.Email,
            userCreatedEvent.RoleType);

        // 這裡可以加入應用層的業務邏輯
        // 例如：創建使用者設定檔、分配預設權限等

        await Task.CompletedTask;
    }
}