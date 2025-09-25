namespace SoftwareDevelopment.Domain.Common;

/// <summary>
/// 值物件基底類別
/// 提供基於值比較的相等性判斷
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <summary>
    /// 取得用於相等性比較的原子值
    /// </summary>
    /// <returns>原子值序列</returns>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <summary>
    /// 檢查兩個值物件是否相等
    /// </summary>
    /// <param name="other">要比較的值物件</param>
    /// <returns>是否相等</returns>
    public bool Equals(ValueObject? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// 檢查兩個物件是否相等
    /// </summary>
    /// <param name="obj">要比較的物件</param>
    /// <returns>是否相等</returns>
    public override bool Equals(object? obj)
    {
        return obj is ValueObject valueObject && Equals(valueObject);
    }

    /// <summary>
    /// 取得雜湊碼
    /// </summary>
    /// <returns>雜湊碼</returns>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Where(component => component is not null)
            .Aggregate(1, (current, component) => current * 23 + component!.GetHashCode());
    }

    /// <summary>
    /// 相等運算子
    /// </summary>
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// 不相等運算子
    /// </summary>
    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !Equals(left, right);
    }
}
