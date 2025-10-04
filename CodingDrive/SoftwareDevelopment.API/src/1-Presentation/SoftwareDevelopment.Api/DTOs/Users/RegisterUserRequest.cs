using System.ComponentModel.DataAnnotations;

namespace SoftwareDevelopment.Api.DTOs.Users;

public sealed class RegisterUserRequest
{
    [Required(ErrorMessage = "使用者名稱為必填")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "使用者名稱長度必須介於 3-50 字元")]
    public string Username { get; set; } = default!;

    [Required(ErrorMessage = "電子信箱為必填")]
    [EmailAddress(ErrorMessage = "電子信箱格式無效")]
    [StringLength(100, ErrorMessage = "電子信箱長度不能超過 100 字元")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "名字為必填")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "名字長度必須介於 1-50 字元")]
    public string FirstName { get; set; } = default!;

    [Required(ErrorMessage = "姓氏為必填")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "姓氏長度必須介於 1-50 字元")]
    public string LastName { get; set; } = default!;

    [Required(ErrorMessage = "密碼為必填")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "密碼長度必須介於 8-100 字元")]
    public string Password { get; set; } = default!;

    [Required(ErrorMessage = "確認密碼為必填")]
    [Compare(nameof(Password), ErrorMessage = "密碼與確認密碼不相符")]
    public string ConfirmPassword { get; set; } = default!;
}