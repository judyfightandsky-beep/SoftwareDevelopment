using MediatR;
using SoftwareDevelopment.Domain.Common;

namespace SoftwareDevelopment.Application.Common;

/// <summary>
/// 領域事件通知包裝器
/// 將領域事件包裝成 MediatR 通知，以保持 Domain 層的純淨
/// </summary>
/// <typeparam name="TDomainEvent">領域事件類型</typeparam>
public sealed class DomainEventNotification<TDomainEvent> : INotification
    where TDomainEvent : IDomainEvent
{
    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="domainEvent">領域事件</param>
    public DomainEventNotification(TDomainEvent domainEvent)
    {
        DomainEvent = domainEvent ?? throw new ArgumentNullException(nameof(domainEvent));
    }

    /// <summary>
    /// 領域事件
    /// </summary>
    public TDomainEvent DomainEvent { get; }
}