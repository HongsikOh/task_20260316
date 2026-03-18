using System.Globalization;

namespace Employee_Hotline.Application.Interfaces.Parser;

public static class DateNormalizer
{
    private static readonly string[] Formats =
    [
        "yyyy-MM-dd",
        "yyyy.MM.dd",
        "yyyy/MM/dd",
        "yyyyMMdd"
    ];

    /// <summary>
    /// 다양한 날짜 문자열을 받아서 "yyyy-MM-dd" 로 반환.
    /// 파싱 실패 시 원본 문자열 그대로 반환 → Validator에서 에러 처리.
    /// </summary>
    public static string Normalize(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return string.Empty;

        return DateOnly.TryParseExact(
                   raw.Trim(), Formats,
                   CultureInfo.InvariantCulture,
                   DateTimeStyles.None,
                   out var date)
               ? date.ToString("yyyy-MM-dd")
               : raw.Trim();   // 실패 → 원본 유지 → Validator가 잡아줌
    }
}