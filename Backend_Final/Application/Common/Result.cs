namespace Backend_Final.Application.Common
{
    public class Result<T>
    {
        public bool Success { get; private set; }
        public T? Data { get; private set; }
        public string ErrorMessage { get; private set; } = string.Empty;
        public ResultType Type { get; private set; }

        private Result(bool success, T? data, string errorMessage, ResultType type)
        {
            Success = success;
            Data = data;
            ErrorMessage = errorMessage;
            Type = type;
        }

        public static Result<T> Ok(T data)
            => new(true, data, string.Empty, ResultType.Success);

        public static Result<T> Fail(string message, ResultType type)
            => new(false, default, message, type);
    }
}
