using SoftwareDevelopment.Domain.Common;

namespace SoftwareDevelopment.Domain.Tasks.ValueObjects;

/// <summary>
/// 預估工時值物件
/// </summary>
public sealed class EstimatedHours : ValueObject
{
    private EstimatedHours(decimal value)
    {
        Value = value;
    }

    /// <summary>
    /// 工時值
    /// </summary>
    public decimal Value { get; }

    /// <summary>
    /// 建立預估工時
    /// </summary>
    /// <param name="hours">工時值</param>
    /// <returns>預估工時</returns>
    public static EstimatedHours Create(decimal hours)
    {
        if (hours < 0)
        {
            throw new ArgumentException("預估工時不得為負數", nameof(hours));
        }

        if (hours > 1000)
        {
            throw new ArgumentException("預估工時不得超過 1000 小時", nameof(hours));
        }

        return new EstimatedHours(hours);
    }

    /// <summary>
    /// 零工時
    /// </summary>
    public static EstimatedHours Zero => new(0);

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
    public static implicit operator decimal(EstimatedHours estimatedHours) => estimatedHours.Value;

    /// <summary>
    /// 轉換為字串
    /// </summary>
    public override string ToString() => $"{Value:0.##} 小時";
}