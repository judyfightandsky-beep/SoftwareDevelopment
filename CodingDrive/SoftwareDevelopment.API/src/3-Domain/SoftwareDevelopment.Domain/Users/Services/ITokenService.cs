using SoftwareDevelopment.Domain.Users.Entities;
using SoftwareDevelopment.Domain.Users.ValueObjects;

namespace SoftwareDevelopment.Domain.Users.Services;

/// <summary>
/// Token 生成服務介面
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// 產生存取權杖
    /// </summary>
    /// <param name="user">使用者實體</param>
    /// <returns>存取權杖</returns>
    string GenerateAccessToken(User user);

    /// <summary>
    /// 產生重新整理權杖
    /// </summary>
    /// <param name="user">使用者實體</param>
    /// <returns>重新整理權杖</returns>
    string GenerateRefreshToken(User user);

    /// <summary>
    /// 驗證權杖
    /// </summary>
    /// <param name="token">權杖</param>
    /// <returns>是否有效</returns>
    bool ValidateToken(string token);

    /// <summary>
    /// 從權杖取得使用者 ID
    /// </summary>
    /// <param name="token">權杖</param>
    /// <returns>使用者 ID</returns>
    UserId? GetUserIdFromToken(string token);
}