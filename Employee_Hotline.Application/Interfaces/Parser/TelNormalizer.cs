using System.Text.RegularExpressions;

namespace Employee_Hotline.Application.Interfaces.Parser;

public static class TelNormalizer
{
    /// <summary>
    /// 숫자 외 문자를 모두 제거해서 반환
    /// </summary>
    public static string Normalize(string? raw)
        => string.IsNullOrWhiteSpace(raw)
            ? string.Empty
            : Regex.Replace(raw, @"[^0-9]", "");
}