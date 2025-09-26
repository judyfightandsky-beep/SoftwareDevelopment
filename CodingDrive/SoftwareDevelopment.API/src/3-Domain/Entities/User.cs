using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwareDevelopment.Domain.Entities
{
    public enum UserRole
    {
        Guest,
        Employee,
        Manager,
        SystemAdmin
    }

    public enum UserStatus
    {
        PendingVerification,
        PendingApproval,
        Active,
        Suspended,
        Blocked
    }

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; private set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; private set; }

        [Required]
        [EmailAddress]
        [StringLength(254)]
        public string Email { get; private set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; private set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; private set; }

        [Required]
        public string PasswordHash { get; private set; }

        [Required]
        public UserRole Role { get; private set; }

        [Required]
        public UserStatus Status { get; private set; }

        public DateTime? LastLoginAt { get; private set; }

        [Required]
        public DateTime CreatedAt { get; private set; }

        [Required]
        public DateTime LastModifiedAt { get; private set; }

        // Factory method for creating a new user
        public static User Create(
            string username,
            string email,
            string firstName,
            string lastName,
            string passwordHash,
            UserRole role,
            UserStatus initialStatus)
        {
            var now = DateTime.UtcNow;
            return new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                PasswordHash = passwordHash,
                Role = role,
                Status = initialStatus,
                CreatedAt = now,
                LastModifiedAt = now
            };
        }

        // Method to update last login time
        public void UpdateLastLogin()
        {
            LastLoginAt = DateTime.UtcNow;
            LastModifiedAt = DateTime.UtcNow;
        }

        // Method to change user status
        public void UpdateStatus(UserStatus newStatus)
        {
            Status = newStatus;
            LastModifiedAt = DateTime.UtcNow;
        }

        // Method to validate user can login
        public bool CanLogin()
        {
            return Status == UserStatus.Active;
        }

        // Method to get full name
        public string GetFullName() => $"{FirstName} {LastName}";
    }
}