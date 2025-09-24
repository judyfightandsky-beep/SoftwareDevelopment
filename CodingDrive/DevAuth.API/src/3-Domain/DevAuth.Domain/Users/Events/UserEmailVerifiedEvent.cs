using DevAuth.Domain.Common;
using DevAuth.Domain.Shared;

namespace DevAuth.Domain.Users.Events;

/// <summary>
/// 使用者電子信箱驗證事件
/// </summary>
public sealed record UserEmailVerifiedEvent(
    UserId UserId,
    Email Email,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}