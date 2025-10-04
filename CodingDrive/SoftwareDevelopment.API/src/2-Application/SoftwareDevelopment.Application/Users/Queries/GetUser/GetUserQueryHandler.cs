using MediatR;
using SoftwareDevelopment.Application.Common;
using SoftwareDevelopment.Domain.Users.Entities;
using SoftwareDevelopment.Domain.Users.ValueObjects;
using SoftwareDevelopment.Domain.Users.Repositories;

namespace SoftwareDevelopment.Application.Users.Queries.GetUser;

public sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<GetUserResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<Result<GetUserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var userId = UserId.From(request.UserId);
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            return Result<GetUserResponse>.Failure(
                new Error("USER.NOT_FOUND", "使用者不存在"));
        }

        var response = new GetUserResponse
        {
            Id = user.Id,
            Username = user.Username.Value,
            Email = user.Email.Value,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString() ?? "Unknown",
            IsEmailVerified = user.Status == UserStatus.Active, // 假設 Active 狀態表示電子信箱已驗證
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt
        };

        return Result<GetUserResponse>.Success(response);
    }
}