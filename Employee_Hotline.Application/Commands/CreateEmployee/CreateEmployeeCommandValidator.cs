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
    }
}