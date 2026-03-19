namespace CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Domain;

/// <summary>
/// Aggregate Root Base Class
/// เป็น Entity ตัวหลัก (Root) ของกลุ่ม Entity ที่เกี่ยวข้องกัน (Aggregate)
/// และเป็นตัวกลางในการจัดการ Domain Events
/// </summary>
public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Event ทั้งหมดที่เกิดขึ้นกับ Aggregate ตัวนี้
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// บันทึก Event ว่ามีบางอย่างเกิดขึ้น (แต่ยังไม่ถูกทำงาน จนกว่าจะ Save DB)
    /// </summary>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// ล้าง Event หลังจากการ Dispatch สำเร็จแล้ว
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
