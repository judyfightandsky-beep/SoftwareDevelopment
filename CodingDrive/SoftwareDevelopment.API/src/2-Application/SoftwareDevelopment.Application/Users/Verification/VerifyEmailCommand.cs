using MediatR;
using SoftwareDevelopment.Application.Common;
using SoftwareDevelopment.Domain.Users.Entities;
using SoftwareDevelopment.Domain.Users.ValueObjects;
using SoftwareDevelopment.Domain.Users.Repositories;

namespace SoftwareDevelopment.Application.Users.Verification;

public record VerifyEmailCommand(string Token) : IRequest<Result>;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, Result>
{
    private readonly IUserRepository _userRepository;

    public VerifyEmailCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        // Find verification token
        var verificationToken = await _userRepository.GetVerificationTokenAsync(request.Token);

        if (verificationToken == null)
            return Result.Failure(new Error("VERIFICATION.INVALID_TOKEN", "Invalid verification token."));

        if (!verificationToken.IsValid())
            return Result.Failure(new Error("VERIFICATION.TOKEN_EXPIRED", "Token has expired or been used."));

        // Find user and verify email
        var user = await _userRepository.GetByIdAsync(verificationToken.UserId);

        if (user == null)
            return Result.Failure(new Error("USER.NOT_FOUND", "User not found."));

        // Update user status and consume token
        user.VerifyEmail();
        verificationToken.Consume();

        // User repository should save changes automatically or through UnitOfWork
        await _userRepository.UpdateVerificationTokenAsync(verificationToken);

        return Result.Success();
    }
}