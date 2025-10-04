using SoftwareDevelopment.Application.Common;
using MediatR;

namespace SoftwareDevelopment.Application.Users.Queries.GetUser;

public sealed record GetUserQuery(Guid UserId) : IRequest<Result<GetUserResponse>>;