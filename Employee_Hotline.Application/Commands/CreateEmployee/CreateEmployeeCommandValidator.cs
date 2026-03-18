using Employee_Hotline.Application.Validators.Extensions;
using FluentValidation;

namespace Employee_Hotline.Application.Commands.CreateEmployee;

public sealed class CreateEmployeeCommandValidator
    : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("등록할 직원 목록이 비어 있습니다.")
            .Must(items => items.Count <= 100)
            .WithMessage("한 번에 최대 100건까지 등록할 수 있습니다.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.Name)
            .NotEmpty()
            .WithMessage("이름은 필수입니다.")
            .MaximumLength(30)
            .WithMessage("이름은 100자 이하여야 합니다.");

            item.RuleFor(i => i.Email)
                .NotEmpty()
                .WithMessage("이메일은 필수입니다.")
                .EmailAddress()
                .WithMessage("올바른 이메일 형식이 아닙니다.")
                .MaximumLength(100)
                .WithMessage("이메일은 200자 이하여야 합니다.");

            item.RuleFor(i => i.Tel)
                .NotEmpty()
                .WithMessage("전화번호는 필수입니다.")
                .MinimumLength(10)
                .MaximumLength(13)
                .BeValidTel();

            item.RuleFor(i => i.Joined)
                .NotEmpty()
                .WithMessage("입사일은 필수입니다.")
                .BeValidDateFormat();
        });
    }
}