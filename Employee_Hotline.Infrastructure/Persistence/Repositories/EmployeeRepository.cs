using Employee_Hotline.Application.Interfaces;
using Employee_Hotline.Domain.Entities;
using Employee_Hotline.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Employee_Hotline.InfraStructure.Persistence.Repositories;

public sealed class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _ctx;
    public EmployeeRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task AddRangeAsync(
    IEnumerable<Employee> employees, CancellationToken ct = default)
    => await _ctx.Employees.AddRangeAsync(employees, ct);

    public async Task<HashSet<string>> GetExistingNamesAsync(
    IEnumerable<string> names, CancellationToken ct = default)
    => (await _ctx.Employees
           .Where(e => names.Contains(e.Name))
           .Select(e => e.Name)
           .ToListAsync(ct))
       .ToHashSet();
}