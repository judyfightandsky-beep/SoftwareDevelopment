using SoftwareDevelopment.Domain.Common;

namespace SoftwareDevelopment.Domain.Users;

/// <summary>
/// 使用者識別碼值物件
/// </summary>
public sealed class UserId : ValueObject
{
    private UserId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// 識別碼值
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// 建立新的使用者識別碼
    /// </summary>
    /// <returns>新的使用者識別碼</returns>
    public static UserId NewId() => new(Guid.NewGuid());

    /// <summary>
    /// 從 GUID 建立使用者識別碼
    /// </summary>
    /// <param name="value">GUID 值</param>
    /// <returns>使用者識別碼</returns>
    public static UserId From(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("使用者識別碼不得為空值", nameof(value));
        }

        return new UserId(value);
    }

    /// <summary>
    /// 取得用於相等性比較的原子值
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// 隱式轉換為 GUID
    /// </summary>
    public static implicit operator Guid(UserId userId) => userId.Value;

    /// <summary>
    /// 轉換為字串
    /// </summary>
    public override string ToString() => Value.ToString();
}
