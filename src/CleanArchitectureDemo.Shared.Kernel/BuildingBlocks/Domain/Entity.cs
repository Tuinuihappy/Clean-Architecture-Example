namespace CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Domain;

/// <summary>
/// Entity Base Class
/// Entity คือ Object ที่มีความสำคัญที่ Identity (เช่น Id) ไม่ใช่ที่ Value
/// ตราบใดที่ Id ตรงกัน ถือว่าเป็น Entity ตัวเดียวกันแม้ข้อมูลอื่นจะต่างกัน
/// </summary>
public abstract class Entity
{
    public int Id { get; protected set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        if (this == obj)
            return true;

        var other = (Entity)obj;

        // ถ้า Id เป็น 0 ทั้งคู่ แสดงว่าเพิ่ง new ขึ้นมา ยังไม่ลง DB ให้เทียบที่ reference
        if (Id == 0 || other.Id == 0)
            return false;

        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }
}
