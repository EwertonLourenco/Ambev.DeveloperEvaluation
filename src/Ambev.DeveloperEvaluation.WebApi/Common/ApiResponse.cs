using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

public class ApiResponse
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public IEnumerable<ValidationErrorDetail> Errors { get; init; } = Array.Empty<ValidationErrorDetail>();

    public static ApiResponse Ok(string? message = null) =>
        new() { Success = true, Message = message ?? string.Empty };

    public static ApiResponse Error(string message, IEnumerable<ValidationErrorDetail>? errors = null) =>
        new()
        {
            Success = false,
            Message = message,
            Errors = errors ?? Array.Empty<ValidationErrorDetail>()
        };
}