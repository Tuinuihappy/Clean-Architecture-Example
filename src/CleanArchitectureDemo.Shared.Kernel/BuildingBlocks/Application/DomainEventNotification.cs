using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Domain;
using MediatR;

namespace CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application;

/// <summary>
/// Wrapper สำหรับแปลง IDomainEvent (ของ Domain) ให้กลายเป็น INotification (ของ MediatR)
/// เพื่อให้ Domain Layer ไม่ต้องยึดติดกับ MediatR package
/// </summary>
public class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : IDomainEvent
{
    public TDomainEvent DomainEvent { get; }

    public DomainEventNotification(TDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }
}
