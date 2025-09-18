namespace StudentManagementAPI.Models
{
    public class Result
    {
        public Result(bool isSuccess, Error error, string message = "")
        {
            IsSuccess = isSuccess;
            Error = error;
            Message = message;
        }

        public bool IsSuccess { get; }
        public Error Error { get; }
        public string Message { get; }

        public static Result Success() => new(true, Error.None, "Success");
        public static Result Success(string message) => new(true, Error.None, message);

        public static Result Failure(Error error) => new(false, error);

        public static Result<T> Success<T>(T data) => new(true, Error.None, data, "Success");
        public static Result<T> Success<T>(T data, string message) => new(true, Error.None, data, message);

        public static Result<T> Failure<T>(Error error) => new(false, error, default);
    }
}
