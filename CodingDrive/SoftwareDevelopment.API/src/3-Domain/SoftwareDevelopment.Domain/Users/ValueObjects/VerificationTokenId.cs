using SoftwareDevelopment.Domain.Common;

namespace SoftwareDevelopment.Domain.Users.ValueObjects;

/// <summary>
/// 驗證權杖唯一識別符值物件
/// </summary>
public sealed class VerificationTokenId : ValueObject
{
    public Guid Value { get; private set; }

    // EF Core 設計時需要的無參數建構函式
    private VerificationTokenId()
    {
        Value = Guid.Empty;
    }

    private VerificationTokenId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("VerificationTokenId 不能為空", nameof(value));

        Value = value;
    }

    /// <summary>
    /// 建立新的驗證權杖 ID
    /// </summary>
    public static VerificationTokenId CreateNew() => new(Guid.NewGuid());

    /// <summary>
    /// 從現有的 GUID 建立驗證權杖 ID
    /// </summary>
    public static VerificationTokenId From(Guid value) => new(value);

    /// <summary>
    /// 從字串建立驗證權杖 ID
    /// </summary>
    public static VerificationTokenId FromString(string value)
    {
        if (!Guid.TryParse(value, out var guid))
            throw new ArgumentException("無效的 VerificationTokenId 格式", nameof(value));

        return new VerificationTokenId(guid);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    /// <summary>
    /// 隱式轉換為 Guid
    /// </summary>
    public static implicit operator Guid(VerificationTokenId id) => id.Value;

    /// <summary>
    /// 隱式轉換為字串
    /// </summary>
    public static implicit operator string(VerificationTokenId id) => id.Value.ToString();
}