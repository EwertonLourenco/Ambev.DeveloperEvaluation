using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

public class ApiResponseWithData<T> : ApiResponse
{
    public T Data { get; init; } = default!;

    public static ApiResponseWithData<T> Ok(T data, string? message = null) =>
        new() { Success = true, Message = message ?? string.Empty, Data = data };

    public static ApiResponseWithData<T> Error(string message, IEnumerable<ValidationErrorDetail>? errors = null) =>
        new()
        {
            Success = false,
            Message = message,
            Errors = errors ?? Array.Empty<ValidationErrorDetail>(),
            Data = default!
        };
}
