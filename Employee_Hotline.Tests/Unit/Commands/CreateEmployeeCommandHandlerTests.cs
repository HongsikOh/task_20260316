using Employee_Hotline.Application.Commands.CreateEmployee;
using Employee_Hotline.Application.DTOs;
using Employee_Hotline.Application.Interfaces;
using Employee_Hotline.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Employee_Hotline.Tests.Unit.Commands;

public class CreateEmployeeCommandHandlerTests
{
    private readonly Mock<IEmployeeRepository> _repoMock = new();
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly CreateEmployeeCommandHandler _handler;

    public CreateEmployeeCommandHandlerTests()
    {
        _handler = new CreateEmployeeCommandHandler(_repoMock.Object, _uowMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllSuccess()
    {
        var items = new List<CreateEmployeeItemDto>
        {
            new("홍길동", "a@test.com", "01011111111", "2022-01-01"),
            new("김철수", "b@test.com", "01022222222", "2022-02-01")
        };
        var command = new CreateEmployeeCommand(items);

        _repoMock
            .Setup(r => r.GetExistingNamesAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.SuccessCount.Should().Be(2);
        result.FailureCount.Should().Be(0);
        result.Failures.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_PartialSuccess_DuplicateNameInDb()
    {
        var items = new List<CreateEmployeeItemDto>
        {
            new("홍길동", "a@test.com", "01011111111", "2022-01-01"), // 이미 존재
            new("김철수", "b@test.com", "01022222222", "2022-02-01")
        };
        var command = new CreateEmployeeCommand(items);

        _repoMock
            .Setup(r => r.GetExistingNamesAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(["홍길동"]);  // 홍길동은 이미 존재

        var result = await _handler.Handle(command, CancellationToken.None);

        result.SuccessCount.Should().Be(1);
        result.FailureCount.Should().Be(1);
        result.Failures[0].Name.Should().Be("홍길동");
    }

    [Fact]
    public async Task Handle_PartialSuccess_DuplicateNameInRequest()
    {
        var items = new List<CreateEmployeeItemDto>
        {
            new("홍길동", "a@test.com", "01011111111", "2022-01-01"),
            new("홍길동", "b@test.com", "01022222222", "2022-02-01")  // 요청 내 중복
        };
        var command = new CreateEmployeeCommand(items);

        _repoMock
            .Setup(r => r.GetExistingNamesAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.SuccessCount.Should().Be(1);
        result.FailureCount.Should().Be(1);
    }

    [Fact]
    public async Task Handle_EmptyItems()
    {
        var command = new CreateEmployeeCommand([]);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.SuccessCount.Should().Be(0);
        result.FailureCount.Should().Be(0);

        _repoMock.Verify(r => r.AddRangeAsync(
            It.IsAny<IEnumerable<Employee>>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }
}