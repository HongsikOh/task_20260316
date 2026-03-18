using Employee_Hotline.Application.DTOs;

namespace Employee_Hotline.Application.Interfaces.Parser;

public interface IEmployeeFileParser
{
    // Content-Type 또는 파일 확장자로 어떤 파서인지 판별
    bool CanHandle(string contentTypeOrExtension);

    Task<IReadOnlyList<CreateEmployeeItemDto>> ParseAsync(
        Stream stream, CancellationToken ct = default);
}