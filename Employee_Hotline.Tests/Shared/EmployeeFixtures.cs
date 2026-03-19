using Employee_Hotline.Application.Commands.CreateEmployee;
using Employee_Hotline.Application.DTOs;

namespace Employee_Hotline.Tests.Shared;

public static class EmployeeFixtures
{
    public static CreateEmployeeCommand CreateEmployeeCommand(
        string? name = null,
        string? email = null,
        string? tel = null,
        string? joined = null)
    {
        return new CreateEmployeeCommand(
            [
                new CreateEmployeeItemDto(
                    Name: name ?? "홍길동이",
                    Email: email ?? "hong@test.com",
                    Tel: tel ?? "01012345678",
                    Joined: joined ?? "2026-03-01"
                )
            ]
        );
    }

    public static List<CreateEmployeeItemDto> CreateEmployeeRawData(
        string? name = null,
        string? email = null,
        string? tel = null,
        string? joined = null)
    {

        Random rn = new();
        return [
            new CreateEmployeeItemDto(
                Name: name ?? "홍길동이",
                Email: email ?? $"test-{rn.NextInt64(10000)}@test.com",
                Tel: tel ?? $"0101234{rn.NextInt64(9999)}",
                Joined: joined ?? "2026-03-01"
            )
        ];
    }


    public static EmployeeDto CreateEmployeeDto(
        Guid? id = null,
        string? name = null,
        string? email = null,
        string? tel = null)
    {
        return new EmployeeDto(
            Id: id ?? Guid.NewGuid(),
            Name: name ?? "홍길동이",
            Email: email ?? "hong@test.com",
            Tel: tel ?? "01012345678",
            Joined: DateTime.UtcNow.AddYears(-1)
        );
    }
}