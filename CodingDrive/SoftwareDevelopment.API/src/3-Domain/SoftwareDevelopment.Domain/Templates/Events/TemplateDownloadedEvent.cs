using SoftwareDevelopment.Domain.Common;
using SoftwareDevelopment.Domain.Templates.ValueObjects;
using SoftwareDevelopment.Domain.Users.ValueObjects;

namespace SoftwareDevelopment.Domain.Templates.Events;

/// <summary>
/// 模板下載事件
/// </summary>
public sealed record TemplateDownloadedEvent(
    TemplateId TemplateId,
    string Name,
    UserId DownloadedBy,
    int NewDownloadCount,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}