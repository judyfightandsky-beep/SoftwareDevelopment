using System.Threading.Tasks;
using SoftwareDevelopment.Domain.Entities;

namespace SoftwareDevelopment.Domain.Services
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Validates user credentials and returns authenticated user
        /// </summary>
        /// <param name="usernameOrEmail">Username or email for login</param>
        /// <param name="password">User's password</param>
        /// <returns>Authenticated user or null if invalid</returns>
        Task<User> ValidateUserCredentialsAsync(string usernameOrEmail, string password);

        /// <summary>
        /// Generates a verification token for email verification
        /// </summary>
        /// <param name="user">User requiring verification</param>
        /// <returns>Verification token</returns>
        Task<string> GenerateEmailVerificationTokenAsync(User user);

        /// <summary>
        /// Verifies email using the provided token
        /// </summary>
        /// <param name="token">Email verification token</param>
        /// <returns>Verified user or null if token is invalid</returns>
        Task<User> VerifyEmailAsync(string token);

        /// <summary>
        /// Generates a password reset token
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns>Password reset token</returns>
        Task<string> GeneratePasswordResetTokenAsync(string email);

        /// <summary>
        /// Resets user password using reset token
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="token">Password reset token</param>
        /// <param name="newPassword">New password</param>
        /// <returns>True if password reset successful, false otherwise</returns>
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
    }
}