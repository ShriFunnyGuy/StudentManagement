using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagementAPI.Models;

namespace StudentManagement.Infrastructure;

public abstract class ApiControllerBase : ControllerBase
{
    protected ActionResult<ApiResponse<T>> From<T>(Result<T> result)
        => Ok(result.ToApiResponse());

    protected ActionResult<ApiResponse<object?>> From(Result result)
        => Ok(result.ToApiResponse());
}