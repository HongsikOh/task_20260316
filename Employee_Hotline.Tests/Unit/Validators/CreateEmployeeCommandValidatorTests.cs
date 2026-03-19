using Employee_Hotline.Application.Commands.CreateEmployee;
using Employee_Hotline.Application.Validators.DTOValidators;
using Employee_Hotline.Tests.Shared;
using FluentAssertions;

namespace Employee_Hotline.Tests.Unit.Validators;

public class CreateEmployeeCommandValidatorTests
{
    private readonly CreateEmployeeCommandValidator _commandValidator = new();
    private readonly CreateEmployeeItemDtoValidator _dtoValidator = new();

    [Fact]
    public async Task Validate_ValidCommand()
    {
        var command = EmployeeFixtures.CreateEmployeeCommand();

        var result = await _commandValidator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Validate_EmptyName(string? name)
    {
        var dto = EmployeeFixtures.CreateEmployeeItemDto(name: name!);
        var result = await _dtoValidator.ValidateAsync(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("이름"));
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("missing@")]
    [InlineData("@nodomain")]
    public async Task Validate_InvalidEmail(string email)
    {
        var dto = EmployeeFixtures.CreateEmployeeItemDto(email: email!);
        var result = await _dtoValidator.ValidateAsync(dto);

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
        var dto = EmployeeFixtures.CreateEmployeeItemDto(tel: tel!);
        var result = await _dtoValidator.ValidateAsync(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("전화번호"));
    }

    [Theory]
    [InlineData("2022-01-01")]
    public async Task Validate_ValidDateFormats(string joined)
    {
        var dto = EmployeeFixtures.CreateEmployeeItemDto(joined: joined!);
        var result = await _dtoValidator.ValidateAsync(dto);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("22-01-01")]
    [InlineData("not-a-date")]
    public async Task Validate_InvalidDateFormats(string joined)
    {
        var dto = EmployeeFixtures.CreateEmployeeItemDto(joined: joined!);
        var result = await _dtoValidator.ValidateAsync(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("날짜"));
    }
}