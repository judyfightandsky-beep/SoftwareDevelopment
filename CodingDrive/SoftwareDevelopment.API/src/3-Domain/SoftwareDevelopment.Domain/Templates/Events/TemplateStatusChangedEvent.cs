using SoftwareDevelopment.Domain.Common;
using SoftwareDevelopment.Domain.Templates.ValueObjects;
using SoftwareDevelopment.Domain.Users.ValueObjects;

namespace SoftwareDevelopment.Domain.Templates.Events;

/// <summary>
/// 模板狀態變更事件
/// </summary>
public sealed record TemplateStatusChangedEvent(
    TemplateId TemplateId,
    TemplateStatus OldStatus,
    TemplateStatus NewStatus,
    UserId ChangedBy,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}