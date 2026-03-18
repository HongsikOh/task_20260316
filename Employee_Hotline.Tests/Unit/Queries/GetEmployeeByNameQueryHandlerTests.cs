using Employee_Hotline.Application.DTOs;
using Employee_Hotline.Application.Interfaces;
using Employee_Hotline.Application.Queries.GetEmployeeByName;
using Employee_Hotline.Tests.Shared;
using FluentAssertions;
using Moq;

namespace Employee_Hotline.Tests.Unit.Queries;

public class GetEmployeeByNameQueryHandlerTests
{
    private readonly Mock<IEmployeeReadRepository> _readMock = new();
    private readonly GetEmployeeByNameQueryHandler _handler;

    public GetEmployeeByNameQueryHandlerTests()
        => _handler = new GetEmployeeByNameQueryHandler(_readMock.Object);

    [Fact]
    public async Task Handle_ExistingName()
    {
        var dto = EmployeeFixtures.CreateEmployeeDto();
        var query = new GetEmployeeByNameQuery(dto.Name);

        _readMock
            .Setup(r => r.GetByNameAsync(dto.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(dto.Id);
        result.Name.Should().Be(dto.Name);
    }

    [Fact]
    public async Task Handle_NotExistingName()
    {
        _readMock
            .Setup(r => r.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((EmployeeDto?)null);

        var result = await _handler.Handle(
            new GetEmployeeByNameQuery("test"), CancellationToken.None);

        result.Should().BeNull();
    }
}