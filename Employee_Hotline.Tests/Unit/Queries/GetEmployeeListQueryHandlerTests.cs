using Employee_Hotline.Application.DTOs;
using Employee_Hotline.Application.Interfaces;
using Employee_Hotline.Application.Queries.GetEmployeeList;
using Employee_Hotline.Tests.Shared;
using FluentAssertions;
using Moq;

namespace Employee_Hotline.Tests.Unit.Queries;

public class GetEmployeeListQueryHandlerTests
{
    private readonly Mock<IEmployeeReadRepository> _readMock = new();
    private readonly GetEmployeeListQueryHandler _handler;

    public GetEmployeeListQueryHandlerTests()
        => _handler = new GetEmployeeListQueryHandler(_readMock.Object);

    [Fact]
    public async Task Handle_ReturnsAllEmployees()
    {
        var dto = new EmployeeListDto(
            new List<EmployeeDto>
            {
                EmployeeFixtures.CreateEmployeeDto(name: "홍길동"),
                EmployeeFixtures.CreateEmployeeDto(name: "김철수")
            }.AsReadOnly(),
            2);

        _readMock
            .Setup(r => r.GetListAsync(1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var result = await _handler.Handle(new GetEmployeeListQuery(1, 10), CancellationToken.None);

        result.TotalCount.Should().Be(2);
        result.Employees.Select(e => e.Name).Should().Contain(["홍길동", "김철수"]);
    }

    [Fact]
    public async Task Handle_NoEmployees()
    {
        _readMock
            .Setup(r => r.GetListAsync(1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EmployeeListDto(new List<EmployeeDto>().AsReadOnly(), 0));

        var result = await _handler.Handle(new GetEmployeeListQuery(1, 10), CancellationToken.None);

        result.TotalCount.Should().Be(0);
        result.Employees.Should().BeEmpty();
    }
}