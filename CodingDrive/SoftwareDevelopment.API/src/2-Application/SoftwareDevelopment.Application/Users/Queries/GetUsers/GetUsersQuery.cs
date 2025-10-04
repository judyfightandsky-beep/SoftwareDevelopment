using SoftwareDevelopment.Application.Common;
using MediatR;

namespace SoftwareDevelopment.Application.Users.Queries.GetUsers;

public sealed record GetUsersQuery(
    int PageNumber = 1,
    int PageSize = 20,
    string? Role = null,
    string? Status = null) : IRequest<Result<GetUsersResponse>>;