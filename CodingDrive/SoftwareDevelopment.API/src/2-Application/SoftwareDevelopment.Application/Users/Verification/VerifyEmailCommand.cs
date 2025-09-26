using MediatR;
using SoftwareDevelopment.Application.Common;
using SoftwareDevelopment.Domain.Repositories;
using SoftwareDevelopment.Domain.Users;

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
            return Result.Failure("Invalid verification token.");

        if (!verificationToken.IsValid())
            return Result.Failure("Token has expired or been used.");

        // Find user and verify email
        var user = await _userRepository.GetByIdAsync(verificationToken.UserId);

        if (user == null)
            return Result.Failure("User not found.");

        // Update user status and consume token
        user.VerifyEmail();
        verificationToken.Consume();

        await _userRepository.UpdateAsync(user);
        await _userRepository.UpdateVerificationTokenAsync(verificationToken);

        return Result.Success("Email verified successfully.");
    }
}