using System.Text.RegularExpressions;
using DevAuth.Domain.Common;

namespace DevAuth.Domain.Shared;

/// <summary>
/// 電子信箱值物件
/// 提供電子信箱驗證和網域判定功能
/// </summary>
public sealed class Email : ValueObject
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private Email(string value)
    {
        Value = value;
    }

    /// <summary>
    /// 電子信箱值
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// 取得網域名稱
    /// </summary>
    public string Domain => Value.Split('@')[1];

    /// <summary>
    /// 建立電子信箱值物件
    /// </summary>
    /// <param name="email">電子信箱字串</param>
    /// <returns>電子信箱值物件</returns>
    /// <exception cref="ArgumentException">無效的電子信箱格式</exception>
    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("電子信箱不得為空值或空白", nameof(email));
        }

        var normalizedEmail = email.Trim().ToLowerInvariant();

        if (!EmailRegex.IsMatch(normalizedEmail))
        {
            throw new ArgumentException("無效的電子信箱格式", nameof(email));
        }

        return new Email(normalizedEmail);
    }

    /// <summary>
    /// 檢查是否為公司信箱
    /// </summary>
    /// <param name="companyDomains">公司網域清單</param>
    /// <returns>是否為公司信箱</returns>
    public bool IsCompanyEmail(IEnumerable<string> companyDomains)
    {
        ArgumentNullException.ThrowIfNull(companyDomains);

        return companyDomains.Any(domain =>
            Domain.Equals(domain, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// 取得用於相等性比較的原子值
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// 隱式轉換為字串
    /// </summary>
    public static implicit operator string(Email email) => email.Value;

    /// <summary>
    /// 轉換為字串
    /// </summary>
    public override string ToString() => Value;
}