using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SoftwareDevelopment.Domain.Entities;

namespace SoftwareDevelopment.Infrastructure.Services
{
    public interface IJwtTokenService
    {
        /// <summary>
        /// Generate an access token for a user
        /// </summary>
        /// <param name="user">User to generate token for</param>
        /// <returns>JWT token string</returns>
        string GenerateAccessToken(User user);

        /// <summary>
        /// Generate a refresh token for a user
        /// </summary>
        /// <param name="user">User to generate token for</param>
        /// <returns>Refresh token string</returns>
        string GenerateRefreshToken(User user);

        /// <summary>
        /// Validate a JWT token
        /// </summary>
        /// <param name="token">Token to validate</param>
        /// <returns>ClaimsPrincipal if valid, null otherwise</returns>
        ClaimsPrincipal ValidateToken(string token);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _secretKey;
        private readonly int _accessTokenExpiryMinutes;
        private readonly int _refreshTokenExpiryDays;

        public JwtTokenService(
            string issuer,
            string audience,
            string secretKey,
            int accessTokenExpiryMinutes = 15,
            int refreshTokenExpiryDays = 7)
        {
            _issuer = issuer;
            _audience = audience;
            _secretKey = secretKey;
            _accessTokenExpiryMinutes = accessTokenExpiryMinutes;
            _refreshTokenExpiryDays = refreshTokenExpiryDays;
        }

        public string GenerateAccessToken(User user)
        {
            return GenerateToken(user, _accessTokenExpiryMinutes);
        }

        public string GenerateRefreshToken(User user)
        {
            return GenerateToken(user, _accessTokenExpiryMinutes * _refreshTokenExpiryDays, isRefreshToken: true);
        }

        private string GenerateToken(User user, int expiryMinutes, bool isRefreshToken = false)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("status", user.Status.ToString())
            };

            // Add additional permissions based on role
            switch (user.Role)
            {
                case UserRole.SystemAdmin:
                    claims.Add(new Claim("permissions", "FULL_ACCESS"));
                    break;
                case UserRole.Manager:
                    claims.AddRange(new[]
                    {
                        new Claim("permissions", "READ_PROJECTS"),
                        new Claim("permissions", "WRITE_CODE"),
                        new Claim("permissions", "APPROVE_CODE")
                    });
                    break;
                case UserRole.Employee:
                    claims.AddRange(new[]
                    {
                        new Claim("permissions", "READ_PROJECTS"),
                        new Claim("permissions", "WRITE_CODE")
                    });
                    break;
                default:
                    claims.Add(new Claim("permissions", "READ_ONLY"));
                    break;
            }

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1)
            };

            try
            {
                return tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch
            {
                return null;
            }
        }
    }
}