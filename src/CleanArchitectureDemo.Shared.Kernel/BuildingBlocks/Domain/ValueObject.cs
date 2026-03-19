namespace CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Domain;

/// <summary>
/// Value Object Base Class
/// Value Object คือ Object ที่ไม่มี Identity ของตัวเอง
/// ความแตกต่างขึ้นอยู่กับ Value ของ properties หาก property เหมือนกันทุกตัว = คือ Object ตัวเดียวกัน
/// เช่น Money(amount, currency), Address(street, city, zip)
/// </summary>
public abstract class ValueObject
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }
}
