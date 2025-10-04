using SoftwareDevelopment.Domain.Common;
using SoftwareDevelopment.Domain.Templates.ValueObjects;
using SoftwareDevelopment.Domain.Users.ValueObjects;

namespace SoftwareDevelopment.Domain.Templates.Events;

/// <summary>
/// 模板建立事件
/// </summary>
public sealed record TemplateCreatedEvent(
    TemplateId TemplateId,
    string Name,
    TemplateType Type,
    UserId CreatedBy,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}