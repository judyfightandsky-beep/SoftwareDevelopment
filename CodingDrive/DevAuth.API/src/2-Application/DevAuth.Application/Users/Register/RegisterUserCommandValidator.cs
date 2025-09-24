using FluentValidation;

namespace DevAuth.Application.Users.Register;

/// <summary>
/// 註冊使用者命令驗證器
/// </summary>
public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    /// <summary>
    /// 初始化註冊使用者命令驗證器
    /// </summary>
    public RegisterUserCommandValidator()
    {
        // 使用者名稱驗證
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("使用者名稱不得為空")
            .Length(3, 20)
            .WithMessage("使用者名稱長度必須介於 3-20 個字元")
            .Matches(@"^[a-zA-Z0-9_]+$")
            .WithMessage("使用者名稱只能包含字母、數字和底線");

        // 電子信箱驗證
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("電子信箱不得為空")
            .EmailAddress()
            .WithMessage("請輸入有效的電子信箱格式")
            .MaximumLength(255)
            .WithMessage("電子信箱長度不得超過 255 個字元");

        // 名字驗證
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("名字不得為空")
            .MaximumLength(50)
            .WithMessage("名字長度不得超過 50 個字元");

        // 姓氏驗證
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("姓氏不得為空")
            .MaximumLength(50)
            .WithMessage("姓氏長度不得超過 50 個字元");

        // 密碼驗證
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("密碼不得為空")
            .MinimumLength(8)
            .WithMessage("密碼長度至少需要 8 個字元")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)")
            .WithMessage("密碼必須包含至少一個小寫字母、一個大寫字母和一個數字");

        // 確認密碼驗證
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage("確認密碼不得為空")
            .Equal(x => x.Password)
            .WithMessage("密碼確認不一致");
    }
}