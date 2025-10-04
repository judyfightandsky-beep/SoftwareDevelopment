using SoftwareDevelopment.Domain.Common;

namespace SoftwareDevelopment.Domain.Tasks.ValueObjects;

/// <summary>
/// 實際工時值物件
/// </summary>
public sealed class ActualHours : ValueObject
{
    private ActualHours(decimal value)
    {
        Value = value;
    }

    /// <summary>
    /// 工時值
    /// </summary>
    public decimal Value { get; }

    /// <summary>
    /// 建立實際工時
    /// </summary>
    /// <param name="hours">工時值</param>
    /// <returns>實際工時</returns>
    public static ActualHours Create(decimal hours)
    {
        if (hours < 0)
        {
            throw new ArgumentException("實際工時不得為負數", nameof(hours));
        }

        if (hours > 1000)
        {
            throw new ArgumentException("實際工時不得超過 1000 小時", nameof(hours));
        }

        return new ActualHours(hours);
    }

    /// <summary>
    /// 零工時
    /// </summary>
    public static ActualHours Zero => new(0);

    /// <summary>
    /// 取得用於相等性比較的原子值
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// 隱式轉換為 decimal
    /// </summary>
    public static implicit operator decimal(ActualHours actualHours) => actualHours.Value;

    /// <summary>
    /// 轉換為字串
    /// </summary>
    public override string ToString() => $"{Value:0.##} 小時";
}