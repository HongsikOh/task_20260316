using Employee_Hotline.Domain.Entities;

namespace Employee_Hotline.Application.Interfaces;

public interface IEmployeeRepository
{
    Task AddRangeAsync(IEnumerable<Employee> employees, CancellationToken ct = default);
    Task<HashSet<string>> GetExistingNamesAsync(IEnumerable<string> names, CancellationToken ct = default);
}