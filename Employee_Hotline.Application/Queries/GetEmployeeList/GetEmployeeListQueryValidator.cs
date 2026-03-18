using FluentValidation;

namespace Employee_Hotline.Application.Queries.GetEmployeeList;

public sealed class GetEmployeeListQueryValidator
    : AbstractValidator<GetEmployeeListQuery>
{
    public GetEmployeeListQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(10000);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(100);
    }
}