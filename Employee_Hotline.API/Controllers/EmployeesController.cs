using Employee_Hotline.Application.Commands.CreateEmployee;
using Employee_Hotline.Application.DTOs;
using Employee_Hotline.Application.Interfaces.Parser;
using Employee_Hotline.Application.Queries.GetEmployeeByName;
using Employee_Hotline.Application.Queries.GetEmployeeList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Hotline.API.Controllers;

[ApiController]
[Route("api/employee")]
public sealed class EmployeesController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IEnumerable<IEmployeeFileParser> _parsers;

    public EmployeesController(
        ISender sender,
        IEnumerable<IEmployeeFileParser> parsers)
    {
        _sender = sender;
        _parsers = parsers;
    }

    #region Queries

    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] int page,
        [FromQuery] int pageSize,
        CancellationToken ct)
        => Ok(await _sender.Send(new GetEmployeeListQuery(page, pageSize), ct));

    [HttpGet("{name}")]
    public async Task<IActionResult> GetByName(string name, CancellationToken ct)
    {
        var result = await _sender.Send(new GetEmployeeByNameQuery(name), ct);
        return result is null ? NotFound() : Ok(result);
    }

    #endregion

    #region Commands

    [HttpPost]
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        IReadOnlyList<CreateEmployeeItemDto> items;

        // file upload
        if (Request.HasFormContentType && Request.Form.Files.Count > 0)
        {
            var file = Request.Form.Files[0];
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var parser = ResolveParser(extension);

            await using var stream = file.OpenReadStream();
            items = await parser.ParseAsync(stream, ct);
        }
        // Raw Body
        else
        {
            var contentType = Request.ContentType?.Split(';')[0].Trim() ?? string.Empty;
            var parser = ResolveParser(contentType);

            items = await parser.ParseAsync(Request.Body, ct);
        }

        var command = new CreateEmployeeCommand(items);
        var result = await _sender.Send(command, ct);

        // 전체 실패면 400, 부분/전체 성공이면 201
        return result switch
        {
            { FailureCount: > 0, SuccessCount: 0 } => BadRequest(result),
            _ => StatusCode(201, result)
        };
    }

    private IEmployeeFileParser ResolveParser(string contentTypeOrExtension)
    {
        var parser = _parsers.FirstOrDefault(p => p.CanHandle(contentTypeOrExtension))
            ?? throw new NotSupportedException(
                $"지원하지 않는 형식입니다: {contentTypeOrExtension}. (지원: text/csv, application/json, .csv, .json)");

        return parser;
    }

    #endregion
}