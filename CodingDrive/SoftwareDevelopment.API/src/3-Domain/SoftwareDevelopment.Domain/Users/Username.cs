using System.Text.RegularExpressions;
using SoftwareDevelopment.Domain.Common;

namespace SoftwareDevelopment.Domain.Users;

/// <summary>
/// 使用者名稱值物件
/// </summary>
public sealed class Username : ValueObject
{
    private static readonly Regex UsernameRegex = new(
        @"^[a-zA-Z0-9_]{3,20}$",
        RegexOptions.Compiled);

    private Username(string value)
    {
        Value = value;
    }

    /// <summary>
    /// 使用者名稱值
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// 建立使用者名稱值物件
    /// </summary>
    /// <param name="username">使用者名稱字串</param>
    /// <returns>使用者名稱值物件</returns>
    /// <exception cref="ArgumentException">無效的使用者名稱格式</exception>
    public static Username Create(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("使用者名稱不得為空值或空白", nameof(username));
        }

        var trimmedUsername = username.Trim();

        if (!UsernameRegex.IsMatch(trimmedUsername))
        {
            throw new ArgumentException(
                "使用者名稱只能包含字母、數字和底線，長度必須介於 3-20 個字元",
                nameof(username));
        }

        return new Username(trimmedUsername);
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
    public static implicit operator string(Username username) => username.Value;

    /// <summary>
    /// 轉換為字串
    /// </summary>
    public override string ToString() => Value;
}
