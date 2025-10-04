using SoftwareDevelopment.Domain.Common;
using SoftwareDevelopment.Domain.Users.ValueObjects;

namespace SoftwareDevelopment.Domain.Users.Entities;

public class VerificationToken : Entity<VerificationTokenId>
{
    public new VerificationTokenId Id { get; private set; }
    public UserId UserId { get; private set; }
    public string Token { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsUsed { get; private set; }

    private VerificationToken() : base(VerificationTokenId.CreateNew())
    {
        // Required for EF Core
        Id = base.Id;
        UserId = null!;
        Token = null!;
    }

    public VerificationToken(UserId userId, string token, TimeSpan validityPeriod) : base(VerificationTokenId.CreateNew())
    {
        Id = base.Id;
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