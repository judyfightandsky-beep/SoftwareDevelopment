namespace SoftwareDevelopment.Application.Users.Queries.GetUsers;

public sealed record GetUsersResponse
{
    public IReadOnlyList<UserItemResponse> Users { get; init; } = new List<UserItemResponse>();
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public bool HasPreviousPage { get; init; }
    public bool HasNextPage { get; init; }
}

public sealed record UserItemResponse
{
    public Guid Id { get; init; }
    public string Username { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Role { get; init; } = default!;
    public bool IsEmailVerified { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? LastLoginAt { get; init; }
}