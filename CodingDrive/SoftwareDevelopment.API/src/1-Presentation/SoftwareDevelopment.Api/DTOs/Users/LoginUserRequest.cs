using System.ComponentModel.DataAnnotations;

namespace SoftwareDevelopment.Api.DTOs.Users;

public sealed record LoginUserRequest
{
    [Required(ErrorMessage = "電子信箱為必填")]
    [EmailAddress(ErrorMessage = "電子信箱格式無效")]
    public string Email { get; init; } = default!;

    [Required(ErrorMessage = "密碼為必填")]
    public string Password { get; init; } = default!;
}