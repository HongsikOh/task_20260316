using Employee_Hotline.Application.DTOs;

namespace Employee_Hotline.Application.Interfaces;

public interface IEmployeeReadRepository
{
    Task<EmployeeDto?> GetByNameAsync(string name, CancellationToken ct = default);
    Task<EmployeeListDto> GetListAsync(int page, int pageSize, CancellationToken ct = default);
}
