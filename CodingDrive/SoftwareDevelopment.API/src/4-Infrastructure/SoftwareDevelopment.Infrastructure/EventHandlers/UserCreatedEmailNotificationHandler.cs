using MediatR;
using Microsoft.Extensions.Logging;
using SoftwareDevelopment.Application.Common;
using SoftwareDevelopment.Domain.Users.Events;
using SoftwareDevelopment.Domain.Users.ValueObjects;

namespace SoftwareDevelopment.Infrastructure.EventHandlers;

/// <summary>
/// 使用者建立郵件通知處理器
/// 處理使用者建立後的外部服務通知（如發送歡迎郵件）
/// </summary>
public sealed class UserCreatedEmailNotificationHandler : INotificationHandler<DomainEventNotification<UserCreatedEvent>>
{
    private readonly ILogger<UserCreatedEmailNotificationHandler> _logger;
    // private readonly IEmailService _emailService; // 未來可以注入郵件服務

    public UserCreatedEmailNotificationHandler(ILogger<UserCreatedEmailNotificationHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// 處理使用者建立事件 - 發送歡迎郵件
    /// </summary>
    /// <param name="notification">領域事件通知包裝器</param>
    /// <param name="cancellationToken">取消權杖</param>
    public async Task Handle(DomainEventNotification<UserCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var userCreatedEvent = notification.DomainEvent;

        _logger.LogInformation(
            "準備為新使用者發送歡迎郵件: UserId={UserId}, Email={Email}, RoleType={RoleType}",
            userCreatedEvent.UserId,
            userCreatedEvent.Email,
            userCreatedEvent.RoleType);

        try
        {
            // 根據角色類型決定郵件內容
            var emailTemplate = userCreatedEvent.RoleType == UserRoleType.Employee
                ? "welcome-employee"
                : "welcome-guest";

            // 模擬發送郵件
            // await _emailService.SendWelcomeEmailAsync(userCreatedEvent.Email, emailTemplate, cancellationToken);

            _logger.LogInformation(
                "歡迎郵件發送成功: Email={Email}, Template={Template}",
                userCreatedEvent.Email,
                emailTemplate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "發送歡迎郵件失敗: UserId={UserId}, Email={Email}",
                userCreatedEvent.UserId,
                userCreatedEvent.Email);

            // 注意：不要重新拋出異常，因為郵件發送失敗不應該影響用戶創建
        }

        await Task.CompletedTask;
    }
}