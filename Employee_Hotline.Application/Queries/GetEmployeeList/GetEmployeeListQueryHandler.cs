using Employee_Hotline.Application.DTOs;
using Employee_Hotline.Application.Interfaces;
using MediatR;

namespace Employee_Hotline.Application.Queries.GetEmployeeList;

public sealed class GetEmployeeListQueryHandler
    : IRequestHandler<GetEmployeeListQuery, EmployeeListDto>
{
    private readonly IEmployeeReadRepository _read;

    public GetEmployeeListQueryHandler(IEmployeeReadRepository read) => _read = read;

    public Task<EmployeeListDto> Handle(
        GetEmployeeListQuery req, CancellationToken ct)
        => _read.GetListAsync(req.Page, req.PageSize, ct);
}