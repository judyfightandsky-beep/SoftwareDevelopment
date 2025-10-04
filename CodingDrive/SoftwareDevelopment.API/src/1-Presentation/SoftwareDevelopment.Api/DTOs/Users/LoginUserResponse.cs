namespace SoftwareDevelopment.Api.DTOs.Users;

public sealed record LoginUserResponse
{
    public string AccessToken { get; init; } = default!;
    public string RefreshToken { get; init; } = default!;
    public string TokenType { get; init; } = "Bearer";
    public int ExpiresIn { get; init; }
    public UserResponse User { get; init; } = default!;
}