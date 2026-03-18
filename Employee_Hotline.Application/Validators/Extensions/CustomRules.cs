using Employee_Hotline.Application.Validators.PropertyValidators;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Employee_Hotline.Application.Validators.Extensions;

public static class CustomRules
{
    /// <summary>
    /// 010으로 시작하는 11자리 숫자인지 검사
    /// 하이픈 등 숫자 외 문자는 검사 전에 제거하고 판단
    /// </summary>
    public static IRuleBuilderOptions<T, string> BeValidTel<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(tel =>
            {
                if (string.IsNullOrWhiteSpace(tel)) return false;

                var digits = Regex.Replace(tel, @"[^0-9]", "");

                return digits.Length == 11 && digits.StartsWith("010");
            })
            .WithMessage("전화번호는 010으로 시작하는 11자리여야 합니다. (예: 01012345678)");
    }

    /// <summary>
    /// joined - string 형식이 yyyy-MM-dd 인지 판단
    /// </summary>
    public static IRuleBuilderOptions<T, string> BeValidDateFormat<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        string format = "yyyy-MM-dd")
    {
        return ruleBuilder.SetValidator(new DateFormatValidator<T>(format));
    }
}