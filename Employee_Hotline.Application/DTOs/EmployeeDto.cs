namespace Employee_Hotline.Application.DTOs;

public record EmployeeDto(
    Guid Id,
    string Name,
    string Email,
    string Tel,
    DateTime Joined
);

public record EmployeeListDto
(
    IReadOnlyList<EmployeeDto> Employees,
    int TotalCount
);

public record CreateEmployeeItemDto(
    string Name,
    string Email,
    string Tel,
    string Joined
);