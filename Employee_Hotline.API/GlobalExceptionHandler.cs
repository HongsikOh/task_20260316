using Employee_Hotline.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace Employee_Hotline.API;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        => _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext ctx, Exception exception, CancellationToken ct)
    {
        var (status, title) = exception switch
        {
            NotFoundException => (404, "리소스를 찾을 수 없습니다."),
            ValidationException => (400, "유효성 검사 실패"),
            _ => (500, "서버 내부 오류")
        };

        _logger.LogError(exception, "[{Status}] {Message}", status, exception.Message);

        ctx.Response.StatusCode = status;
        ctx.Response.ContentType = "application/json";

        await ctx.Response.WriteAsJsonAsync(new
        {
            title,
            status,
            detail = exception.Message
        }, ct);

        return true;
    }
}