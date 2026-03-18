using Employee_Hotline.Application.DTOs;
using Employee_Hotline.Application.Interfaces.Parser;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace Employee_Hotline.InfraStructure.Parsers;

public sealed class CsvEmployeeParser : IEmployeeFileParser
{
    public bool CanHandle(string contentTypeOrExtension)
        => contentTypeOrExtension is "text/csv" or ".csv";

    public async Task<IReadOnlyList<CreateEmployeeItemDto>> ParseAsync(
        Stream stream, CancellationToken ct = default)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            TrimOptions = TrimOptions.Trim,
            MissingFieldFound = null
        });

        var records = new List<CreateEmployeeItemDto>();
        var rowIndex = 0;

        await foreach (var row in csv.GetRecordsAsync<dynamic>(ct))
        {
            rowIndex++;
            try
            {
                // 순서 : 0=Name, 1=Email, 2=Tel, 3=Joined
                var fields = (IDictionary<string, object>)row;
                var vals = fields.Values.Select(v => v?.ToString() ?? "").ToArray();

                if (vals.Length < 4)
                    throw new InvalidDataException($"{rowIndex}행: 필드가 부족합니다. (필수: 이름, 이메일, 전화번호, 입사일)");

                records.Add(new CreateEmployeeItemDto(
                    Name: vals[0],
                    Email: vals[1],
                    Tel: TelNormalizer.Normalize(vals[2]),
                    Joined: DateNormalizer.Normalize(vals[3])
                ));
            }
            catch (InvalidDataException) { throw; }
            catch (Exception ex)
            {
                throw new InvalidDataException($"{rowIndex}행 파싱 실패: {ex.Message}");
            }
        }

        return records.AsReadOnly();
    }
}