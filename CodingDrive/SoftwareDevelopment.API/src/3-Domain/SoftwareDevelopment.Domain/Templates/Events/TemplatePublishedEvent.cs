using SoftwareDevelopment.Domain.Common;
using SoftwareDevelopment.Domain.Templates.ValueObjects;
using SoftwareDevelopment.Domain.Users.ValueObjects;

namespace SoftwareDevelopment.Domain.Templates.Events;

/// <summary>
/// 模板發布事件
/// </summary>
public sealed record TemplatePublishedEvent(
    TemplateId TemplateId,
    string Name,
    string Version,
    UserId PublishedBy,
    DateTime PublishedAt,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}