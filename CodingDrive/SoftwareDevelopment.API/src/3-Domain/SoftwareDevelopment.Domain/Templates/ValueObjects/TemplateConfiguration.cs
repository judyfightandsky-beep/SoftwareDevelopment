using SoftwareDevelopment.Domain.Common;

namespace SoftwareDevelopment.Domain.Templates.ValueObjects;

/// <summary>
/// 模板配置值物件
/// </summary>
public sealed class TemplateConfiguration : ValueObject
{
    private TemplateConfiguration(string configurationJson)
    {
        ConfigurationJson = configurationJson;
    }

    /// <summary>
    /// 配置 JSON 字串
    /// </summary>
    public string ConfigurationJson { get; }

    /// <summary>
    /// 建立模板配置
    /// </summary>
    /// <param name="configurationJson">配置 JSON 字串</param>
    /// <returns>模板配置</returns>
    public static TemplateConfiguration Create(string configurationJson)
    {
        if (string.IsNullOrWhiteSpace(configurationJson))
        {
            throw new ArgumentException("配置 JSON 不得為空", nameof(configurationJson));
        }

        // 這裡可以加入 JSON 格式驗證
        return new TemplateConfiguration(configurationJson);
    }

    /// <summary>
    /// 空配置
    /// </summary>
    public static TemplateConfiguration Empty => new("{}");

    /// <summary>
    /// 取得用於相等性比較的原子值
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ConfigurationJson;
    }

    /// <summary>
    /// 隱式轉換為字串
    /// </summary>
    public static implicit operator string(TemplateConfiguration configuration) => configuration.ConfigurationJson;

    /// <summary>
    /// 轉換為字串
    /// </summary>
    public override string ToString() => ConfigurationJson;
}