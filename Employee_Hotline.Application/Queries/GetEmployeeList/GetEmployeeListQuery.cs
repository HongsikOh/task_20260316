using Employee_Hotline.Application.DTOs;
using MediatR;

namespace Employee_Hotline.Application.Queries.GetEmployeeList;

public record GetEmployeeListQuery(
    int Page,
    int PageSize
) : IRequest<EmployeeListDto>;