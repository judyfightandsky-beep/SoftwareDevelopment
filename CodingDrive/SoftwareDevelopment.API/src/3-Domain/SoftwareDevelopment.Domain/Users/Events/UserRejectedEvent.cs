using SoftwareDevelopment.Domain.Common;

namespace SoftwareDevelopment.Domain.Users.Events;

/// <summary>
/// 使用者拒絕事件
/// </summary>
public sealed record UserRejectedEvent(
    UserId UserId,
    UserId RejectedBy,
    string Reason,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}
