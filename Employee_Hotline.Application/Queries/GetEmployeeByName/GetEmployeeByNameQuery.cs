using Employee_Hotline.Application.DTOs;
using MediatR;

namespace Employee_Hotline.Application.Queries.GetEmployeeByName;

public record GetEmployeeByNameQuery(string Name) : IRequest<EmployeeDto?>;