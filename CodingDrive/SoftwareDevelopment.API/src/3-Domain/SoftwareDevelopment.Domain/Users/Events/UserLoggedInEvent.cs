using SoftwareDevelopment.Domain.Common;

namespace SoftwareDevelopment.Domain.Users.Events;

/// <summary>
/// 使用者登入事件
/// </summary>
public sealed record UserLoggedInEvent(
    UserId UserId,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}
