using SoftwareDevelopment.Domain.Common;
using SoftwareDevelopment.Domain.Shared;
using SoftwareDevelopment.Domain.Users.ValueObjects;

namespace SoftwareDevelopment.Domain.Users.Events;

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
