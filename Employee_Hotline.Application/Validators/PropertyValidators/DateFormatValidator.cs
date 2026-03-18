using FluentValidation;
using FluentValidation.Validators;
using System.Globalization;

namespace Employee_Hotline.Application.Validators.PropertyValidators;

public sealed class DateFormatValidator<T>
    : PropertyValidator<T, string>
{
    private readonly string _format;

    public DateFormatValidator(string format = "yyyy-MM-dd")
        => _format = format;

    public override string Name => "DateFormatValidator";

    public override bool IsValid(
        ValidationContext<T> context, string value)
    {
        var isValid = DateOnly.TryParseExact(
            value, _format,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _);

        if (!isValid)
        {
            context.MessageFormatter
                   .AppendArgument("Format", _format)
                   .AppendArgument("Value", value);
        }

        return isValid;
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "'{Value}' 은 올바른 날짜 형식이 아닙니다. {Format} 형식이어야 합니다.";
}