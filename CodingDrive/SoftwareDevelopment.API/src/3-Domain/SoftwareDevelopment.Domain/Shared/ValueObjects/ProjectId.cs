using SoftwareDevelopment.Domain.Common;

namespace SoftwareDevelopment.Domain.Shared.ValueObjects;

/// <summary>
/// 專案唯一識別符值物件
/// </summary>
public sealed class ProjectId : ValueObject
{
    public Guid Value { get; }

    private ProjectId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("ProjectId 不能為空", nameof(value));

        Value = value;
    }

    /// <summary>
    /// 建立新的專案 ID
    /// </summary>
    public static ProjectId CreateNew() => new(Guid.NewGuid());

    /// <summary>
    /// 從現有的 GUID 建立專案 ID
    /// </summary>
    public static ProjectId From(Guid value) => new(value);

    /// <summary>
    /// 從字串建立專案 ID
    /// </summary>
    public static ProjectId FromString(string value)
    {
        if (!Guid.TryParse(value, out var guid))
            throw new ArgumentException("無效的 ProjectId 格式", nameof(value));

        return new ProjectId(guid);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    /// <summary>
    /// 隱式轉換為 Guid
    /// </summary>
    public static implicit operator Guid(ProjectId projectId) => projectId.Value;

    /// <summary>
    /// 隱式轉換為字串
    /// </summary>
    public static implicit operator string(ProjectId projectId) => projectId.Value.ToString();
}