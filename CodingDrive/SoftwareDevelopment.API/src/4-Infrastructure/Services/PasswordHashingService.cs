using System;
using System.Text.RegularExpressions;
using BCrypt.Net;

namespace SoftwareDevelopment.Infrastructure.Services
{
    public interface IPasswordHashingService
    {
        /// <summary>
        /// Hash a password using BCrypt
        /// </summary>
        /// <param name="password">Plain text password</param>
        /// <returns>Hashed password</returns>
        string HashPassword(string password);

        /// <summary>
        /// Verify a password against its hash
        /// </summary>
        /// <param name="password">Plain text password</param>
        /// <param name="hashedPassword">Stored hashed password</param>
        /// <returns>True if password matches, false otherwise</returns>
        bool VerifyPassword(string password, string hashedPassword);

        /// <summary>
        /// Check if a password meets complexity requirements
        /// </summary>
        /// <param name="password">Password to validate</param>
        /// <returns>True if password meets requirements, false otherwise</returns>
        bool IsPasswordValid(string password);
    }

    public class PasswordHashingService : IPasswordHashingService
    {
        private const int WorkFactor = 12;
        private static readonly Regex PasswordRegex = new Regex(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,100}$",
            RegexOptions.Compiled);

        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty", nameof(password));

            return BCrypt.HashPassword(password, WorkFactor);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword))
                return false;

            return BCrypt.Verify(password, hashedPassword);
        }

        public bool IsPasswordValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Check against known weak passwords
            string[] weakPasswords = { "password", "123456", "qwerty", "admin" };
            if (Array.Exists(weakPasswords, p => p.Equals(password, StringComparison.OrdinalIgnoreCase)))
                return false;

            // Use regex to validate password complexity
            return PasswordRegex.IsMatch(password);
        }
    }
}