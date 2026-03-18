using FluentValidation;

namespace Employee_Hotline.Application.Queries.GetEmployeeByName;

public sealed class GetEmployeeByNameQueryValidator
    : AbstractValidator<GetEmployeeByNameQuery>
{
    public GetEmployeeByNameQueryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("조회할 직원 이름은 필수입니다.")
            .MaximumLength(30)
            .WithMessage("이름은 30자 이하여야 합니다.");
    }
}