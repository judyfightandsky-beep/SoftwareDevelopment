using System;
using System.Threading.Tasks;
using SoftwareDevelopment.Domain.Entities;

namespace SoftwareDevelopment.Domain.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Get user by unique identifier
        /// </summary>
        /// <param name="userId">User's unique identifier</param>
        /// <returns>User or null if not found</returns>
        Task<User> GetByIdAsync(Guid userId);

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username">Username to search</param>
        /// <returns>User or null if not found</returns>
        Task<User> GetByUsernameAsync(string username);

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email">Email to search</param>
        /// <returns>User or null if not found</returns>
        Task<User> GetByEmailAsync(string email);

        /// <summary>
        /// Add a new user to the system
        /// </summary>
        /// <param name="user">User to add</param>
        /// <returns>Added user</returns>
        Task<User> AddAsync(User user);

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="user">User to update</param>
        /// <returns>Updated user</returns>
        Task<User> UpdateAsync(User user);

        /// <summary>
        /// Check if username is already taken
        /// </summary>
        /// <param name="username">Username to check</param>
        /// <returns>True if username exists, false otherwise</returns>
        Task<bool> IsUsernameTakenAsync(string username);

        /// <summary>
        /// Check if email is already registered
        /// </summary>
        /// <param name="email">Email to check</param>
        /// <returns>True if email exists, false otherwise</returns>
        Task<bool> IsEmailTakenAsync(string email);
    }
}