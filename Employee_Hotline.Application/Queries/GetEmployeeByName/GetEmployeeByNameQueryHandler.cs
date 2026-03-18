using Employee_Hotline.Application.DTOs;
using Employee_Hotline.Application.Interfaces;
using MediatR;

namespace Employee_Hotline.Application.Queries.GetEmployeeByName;

public sealed class GetEmployeeByNameQueryHandler
    : IRequestHandler<GetEmployeeByNameQuery, EmployeeDto?>
{
    private readonly IEmployeeReadRepository _read;

    public GetEmployeeByNameQueryHandler(IEmployeeReadRepository read) => _read = read;

    public Task<EmployeeDto?> Handle(GetEmployeeByNameQuery req, CancellationToken ct)
        => _read.GetByNameAsync(req.Name, ct);
}