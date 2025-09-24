using System.Net;
using System.Text.Json;
using FluentValidation;

namespace DevAuth.Api.Middleware;

/// <summary>
/// 全域例外處理中介軟體
/// 統一處理應用程式中的所有未處理例外
/// </summary>
public sealed class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// 初始化全域例外處理中介軟體
    /// </summary>
    /// <param name="next">下一個中介軟體</param>
    /// <param name="logger">日誌記錄器</param>
    public GlobalExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// 執行中介軟體邏輯
    /// </summary>
    /// <param name="httpContext">HTTP 內容</param>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred");
            await HandleExceptionAsync(httpContext, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.ContentType = "application/json";

        var (statusCode, error) = GetErrorResponse(exception);
        httpContext.Response.StatusCode = statusCode;

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var response = JsonSerializer.Serialize(error, jsonOptions);
        await httpContext.Response.WriteAsync(response);
    }

    private static (int StatusCode, ErrorResponse Error) GetErrorResponse(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => (
                (int)HttpStatusCode.BadRequest,
                new ErrorResponse(
                    "VALIDATION_ERROR",
                    "請求資料驗證失敗",
                    validationException.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage))
                )),

            ArgumentException => (
                (int)HttpStatusCode.BadRequest,
                new ErrorResponse(
                    "INVALID_ARGUMENT",
                    exception.Message
                )),

            InvalidOperationException => (
                (int)HttpStatusCode.BadRequest,
                new ErrorResponse(
                    "INVALID_OPERATION",
                    exception.Message
                )),

            UnauthorizedAccessException => (
                (int)HttpStatusCode.Unauthorized,
                new ErrorResponse(
                    "UNAUTHORIZED",
                    "未經授權的存取"
                )),

            _ => (
                (int)HttpStatusCode.InternalServerError,
                new ErrorResponse(
                    "INTERNAL_ERROR",
                    "伺服器內部錯誤，請稍後再試"
                ))
        };
    }
}

/// <summary>
/// 錯誤回應
/// </summary>
/// <param name="Code">錯誤代碼</param>
/// <param name="Message">錯誤訊息</param>
/// <param name="Details">詳細錯誤資訊</param>
public sealed record ErrorResponse(
    string Code,
    string Message,
    IEnumerable<ValidationError>? Details = null);

/// <summary>
/// 驗證錯誤
/// </summary>
/// <param name="Field">欄位名稱</param>
/// <param name="Message">錯誤訊息</param>
public sealed record ValidationError(
    string Field,
    string Message);