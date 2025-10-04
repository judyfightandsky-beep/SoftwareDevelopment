namespace SoftwareDevelopment.Api.DTOs.Users;

public sealed record PagedUsersResponse
{
    public IReadOnlyList<UserResponse> Data { get; init; } = new List<UserResponse>();
    public PaginationMetadata Pagination { get; init; } = default!;
}

public sealed record PaginationMetadata
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
    public bool HasPreviousPage { get; init; }
    public bool HasNextPage { get; init; }
}