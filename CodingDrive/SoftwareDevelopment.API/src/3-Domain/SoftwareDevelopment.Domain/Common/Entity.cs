namespace SoftwareDevelopment.Domain.Common;

/// <summary>
/// 實體基底類別
/// 提供識別碼管理和相等性比較
/// </summary>
/// <typeparam name="TId">實體識別碼類型</typeparam>
public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : class
{
    protected Entity(TId id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }

    /// <summary>
    /// 實體識別碼
    /// </summary>
    public TId Id { get; private set; }

    /// <summary>
    /// 檢查兩個實體是否相等
    /// </summary>
    /// <param name="other">要比較的實體</param>
    /// <returns>是否相等</returns>
    public bool Equals(Entity<TId>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;

        return Id.Equals(other.Id);
    }

    /// <summary>
    /// 檢查兩個物件是否相等
    /// </summary>
    /// <param name="obj">要比較的物件</param>
    /// <returns>是否相等</returns>
    public override bool Equals(object? obj)
    {
        return obj is Entity<TId> entity && Equals(entity);
    }

    /// <summary>
    /// 取得雜湊碼
    /// </summary>
    /// <returns>雜湊碼</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(GetType(), Id);
    }

    /// <summary>
    /// 相等運算子
    /// </summary>
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// 不相等運算子
    /// </summary>
    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
    {
        return !Equals(left, right);
    }
}
