namespace StudentManagement.Models;

public sealed class ApiResponse<T>
{
    public bool Result { get; init; }
    public string Message { get; init; } = string.Empty;
    public T? Data { get; init; }
}