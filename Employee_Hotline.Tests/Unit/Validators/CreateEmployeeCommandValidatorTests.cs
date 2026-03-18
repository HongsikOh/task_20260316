using Employee_Hotline.Application.Commands.CreateEmployee;
using Employee_Hotline.Tests.Shared;
using FluentAssertions;

namespace Employee_Hotline.Tests.Unit.Validators;

public class CreateEmployeeCommandValidatorTests
{
    private readonly CreateEmployeeCommandValidator _validator = new();

    [Fact]
    public async Task Validate_ValidCommand()
    {
        var command = EmployeeFixtures.CreateEmployeeCommand();

        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Validate_EmptyName(string? name)
    {
        var command = EmployeeFixtures.CreateEmployeeCommand(name: name!);
        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("이름"));
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("missing@")]
    [InlineData("@nodomain")]
    public async Task Validate_InvalidEmail(string email)
    {
        var command = EmployeeFixtures.CreateEmployeeCommand(email: email);
        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("이메일"));
    }

    [Theory]
    [InlineData("01112345678")]
    [InlineData("0101234567")]
    [InlineData("010123456789")]
    [InlineData("abcdefghijk")]
    public async Task Validate_InvalidTel(string tel)
    {
        var command = EmployeeFixtures.CreateEmployeeCommand(tel: tel);
        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("전화번호"));
    }

    [Theory]
    [InlineData("2022-01-01")]
    public async Task Validate_ValidDateFormats(string joined)
    {
        var command = EmployeeFixtures.CreateEmployeeCommand(joined: joined);
        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("22-01-01")]
    [InlineData("not-a-date")]
    public async Task Validate_InvalidDateFormats(string joined)
    {
        var command = EmployeeFixtures.CreateEmployeeCommand(joined: joined);
        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("날짜"));
    }
}