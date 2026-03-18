using Employee_Hotline.Application.DTOs;
using MediatR;

namespace Employee_Hotline.Application.Commands.CreateEmployee;

public record CreateEmployeeCommand(
    IReadOnlyList<CreateEmployeeItemDto> Items
) : IRequest<CreateEmployeeResult>;

public record CreateEmployeeResult(
    int SuccessCount,
    int FailureCount,
    IReadOnlyList<CreateEmployeeFailure> Failures
);

public record CreateEmployeeFailure(
    int RowIndex,
    string Name,
    string Reason
);