using SoftwareDevelopment.Domain.Common;

namespace SoftwareDevelopment.Domain.Tasks.ValueObjects;

/// <summary>
/// 任務識別碼值物件
/// </summary>
public sealed class TaskId : ValueObject
{
    private TaskId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// 識別碼值
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// 建立新的任務識別碼
    /// </summary>
    /// <returns>新的任務識別碼</returns>
    public static TaskId NewId() => new(Guid.NewGuid());

    /// <summary>
    /// 從 GUID 建立任務識別碼
    /// </summary>
    /// <param name="value">GUID 值</param>
    /// <returns>任務識別碼</returns>
    public static TaskId From(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("任務識別碼不得為空值", nameof(value));
        }

        return new TaskId(value);
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
    public static implicit operator Guid(TaskId taskId) => taskId.Value;

    /// <summary>
    /// 轉換為字串
    /// </summary>
    public override string ToString() => Value.ToString();
}