using MediatR;
using SoftwareDevelopment.Application.Common;
using SoftwareDevelopment.Domain.Users.Entities;
using SoftwareDevelopment.Domain.Users.ValueObjects;
using SoftwareDevelopment.Domain.Users.Repositories;

namespace SoftwareDevelopment.Application.Users.Queries.GetUsers;

public sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<GetUsersResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<Result<GetUsersResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        // 參數驗證
        if (request.PageNumber < 1)
        {
            return Result<GetUsersResponse>.Failure(
                new Error("QUERY.INVALID_PAGE_NUMBER", "頁數必須大於 0"));
        }

        if (request.PageSize is < 1 or > 100)
        {
            return Result<GetUsersResponse>.Failure(
                new Error("QUERY.INVALID_PAGE_SIZE", "每頁大小必須介於 1-100 之間"));
        }

        // TODO: 實作實際的分頁查詢邏輯
        // 這裡應該調用 repository 的分頁查詢方法
        await Task.CompletedTask;

        // 暫時返回空結果
        var totalCount = 0;
        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        var response = new GetUsersResponse
        {
            Users = new List<UserItemResponse>(),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = totalPages,
            HasPreviousPage = request.PageNumber > 1,
            HasNextPage = request.PageNumber < totalPages
        };

        return Result<GetUsersResponse>.Success(response);
    }
}