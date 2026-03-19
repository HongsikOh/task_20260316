using Employee_Hotline.Application.DTOs;
using Employee_Hotline.Application.Validators.Extensions;
using FluentValidation;

namespace Employee_Hotline.Application.Validators.DTOValidators;

public sealed class CreateEmployeeItemDtoValidator
    : AbstractValidator<CreateEmployeeItemDto>
{
    public CreateEmployeeItemDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("이름은 필수입니다.")
            .MaximumLength(30)
            .WithMessage("이름은 100자 이하여야 합니다.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("이메일은 필수입니다.")
            .EmailAddress()
            .WithMessage("올바른 이메일 형식이 아닙니다.")
            .MaximumLength(100)
            .WithMessage("이메일은 200자 이하여야 합니다.");

        RuleFor(x => x.Tel)
            .NotEmpty()
            .WithMessage("전화번호는 필수입니다.")
            .MinimumLength(10)
            .MaximumLength(13)
            .BeValidTel();

        RuleFor(x => x.Joined)
            .NotEmpty()
            .WithMessage("입사일은 필수입니다.")
            .BeValidDateFormat();
    }
}