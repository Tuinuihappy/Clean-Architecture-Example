namespace CleanArchitectureDemo.Domain.Common;

/// <summary>
/// Domain Event Interface — Marker interface สำหรับ Event ในระบบ
/// เป็นตัวแทนของ "สิ่งที่เกิดขึ้นแล้วใน Domain" (Something that happened in the domain)
/// ใช้ร่วมกับ MediatR INotification เมื่อนำไป Dispatch
/// </summary>
public interface IDomainEvent
{
}
