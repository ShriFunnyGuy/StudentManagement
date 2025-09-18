namespace StudentManagementAPI.Models
{
    public class Result<T> : Result
    {
        public T? Data { get; }
        public Result(bool isSuccess, Error error, T? data, string message = "") : base(isSuccess, error, message)
        {
            Data = data;
        }
    }
}
