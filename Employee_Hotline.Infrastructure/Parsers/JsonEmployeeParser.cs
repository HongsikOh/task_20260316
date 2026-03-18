using Employee_Hotline.Application.DTOs;
using Employee_Hotline.Application.Interfaces.Parser;
using System.Text.Json;

namespace Employee_Hotline.InfraStructure.Parsers;

public sealed class JsonEmployeeParser : IEmployeeFileParser
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public bool CanHandle(string contentTypeOrExtension)
        => contentTypeOrExtension is "application/json" or ".json";

    public async Task<IReadOnlyList<CreateEmployeeItemDto>> ParseAsync(
        Stream stream, CancellationToken ct = default)
    {
        var raws = await JsonSerializer.DeserializeAsync<List<JsonEmployeeRaw>>(
            stream, Options, ct);

        if (raws is null or { Count: 0 })
            throw new InvalidOperationException("JSON 파싱 결과가 비어 있습니다.");

        return raws
            .Select(r => new CreateEmployeeItemDto(
                Name: r.Name,
                Email: r.Email,
                Tel: TelNormalizer.Normalize(r.Tel),
                Joined: DateNormalizer.Normalize(r.Joined)
            ))
            .ToList()
            .AsReadOnly();
    }
}

file sealed class JsonEmployeeRaw
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Tel { get; init; } = string.Empty;
    public string Joined { get; init; } = string.Empty;
}