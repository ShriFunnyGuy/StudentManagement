using StudentManagement.Models;
using StudentManagementAPI.Models;

namespace StudentManagement.Infrastructure;

public static class ResultExtensions
{
    public static ApiResponse<T> ToApiResponse<T>(this Result<T> result, string? messageOverride = null)
        => new ApiResponse<T>
        {
            Result = result.IsSuccess,
            Message = messageOverride ?? (result.IsSuccess ? result.Message : result.Error.Message),
            Data = result.IsSuccess ? result.Data : default
        };

    public static ApiResponse<object?> ToApiResponse(this Result result, string? messageOverride = null)
        => new ApiResponse<object?>
        {
            Result = result.IsSuccess,
            Message = messageOverride ?? (result.IsSuccess ? result.Message : result.Error.Message),
            Data = null
        };
}