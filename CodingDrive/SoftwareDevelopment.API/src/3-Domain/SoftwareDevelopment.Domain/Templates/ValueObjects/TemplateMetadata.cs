using SoftwareDevelopment.Domain.Common;

namespace SoftwareDevelopment.Domain.Templates.ValueObjects;

/// <summary>
/// 模板元數據值物件
/// </summary>
public sealed class TemplateMetadata : ValueObject
{
    private TemplateMetadata(string version, IReadOnlyList<string> tags)
    {
        Version = version;
        Tags = tags;
    }

    /// <summary>
    /// 版本號
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// 標籤列表
    /// </summary>
    public IReadOnlyList<string> Tags { get; }

    /// <summary>
    /// 建立模板元數據
    /// </summary>
    /// <param name="version">版本號</param>
    /// <param name="tags">標籤列表</param>
    /// <returns>模板元數據</returns>
    public static TemplateMetadata Create(string version, IEnumerable<string> tags)
    {
        if (string.IsNullOrWhiteSpace(version))
        {
            throw new ArgumentException("版本號不得為空", nameof(version));
        }

        var tagList = tags?.Where(t => !string.IsNullOrWhiteSpace(t)).ToList() ?? new List<string>();

        return new TemplateMetadata(version, tagList.AsReadOnly());
    }

    /// <summary>
    /// 默認元數據
    /// </summary>
    public static TemplateMetadata Default => new("1.0.0", Array.Empty<string>().AsReadOnly());

    /// <summary>
    /// 取得用於相等性比較的原子值
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Version;
        foreach (var tag in Tags)
        {
            yield return tag;
        }
    }

    /// <summary>
    /// 轉換為字串
    /// </summary>
    public override string ToString() => $"Version: {Version}, Tags: [{string.Join(", ", Tags)}]";
}