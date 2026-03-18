using Employee_Hotline.Application.DTOs;
using Employee_Hotline.Application.Interfaces;
using Employee_Hotline.Domain.Entities;
using Employee_Hotline.InfraStructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Employee_Hotline.InfraStructure.ReadModel;

public sealed class EmployeeReadRepository : IEmployeeReadRepository
{
    private readonly AppDbContext _ctx;
    public EmployeeReadRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<EmployeeDto?> GetByNameAsync(string name, CancellationToken ct = default)
    {
        var e = await _ctx.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name, ct);

        return e is null ? null : MapToDto(e);
    }

    public async Task<EmployeeListDto> GetListAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var list = await _ctx.Employees
            .AsNoTracking()
            .OrderBy(e => e.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var readOnlyList = list.Select(MapToDto).ToList().AsReadOnly();

        var totalCount = await _ctx.Employees
            .AsNoTracking()
            .CountAsync(ct);

        return new(readOnlyList, totalCount);
    }

    private static EmployeeDto MapToDto(Employee e) => new(
        e.Id,
        e.Name,
        e.Email,
        e.Tel,
        e.JoinedAt
    );
}