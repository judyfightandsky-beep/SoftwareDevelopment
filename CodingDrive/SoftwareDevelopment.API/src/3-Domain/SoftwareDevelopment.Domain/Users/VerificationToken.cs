using SoftwareDevelopment.Domain.Common;
using SoftwareDevelopment.Domain.Users;

namespace SoftwareDevelopment.Domain.Users;

public class VerificationToken : Entity
{
    public Guid Id { get; private set; }
    public UserId UserId { get; private set; }
    public string Token { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsUsed { get; private set; }

    private VerificationToken() {} // Required for EF Core

    public VerificationToken(UserId userId, string token, TimeSpan validityPeriod)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Token = token;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = CreatedAt.Add(validityPeriod);
        IsUsed = false;
    }

    public bool IsValid() => !IsUsed && DateTime.UtcNow <= ExpiresAt;

    public void Consume()
    {
        if (!IsValid())
            throw new InvalidOperationException("Token is invalid or expired.");

        IsUsed = true;
    }
}