namespace SoftwareDevelopment.Application.Common;

/// <summary>
/// 密碼雜湊介面
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// 雜湊密碼
    /// </summary>
    /// <param name="password">明文密碼</param>
    /// <returns>雜湊後的密碼</returns>
    string HashPassword(string password);

    /// <summary>
    /// 驗證密碼
    /// </summary>
    /// <param name="hashedPassword">雜湊密碼</param>
    /// <param name="providedPassword">提供的密碼</param>
    /// <returns>是否匹配</returns>
    bool VerifyPassword(string hashedPassword, string providedPassword);
}
