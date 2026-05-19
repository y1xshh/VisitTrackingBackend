namespace VisitTracking.Application.DTOs
{
    public class ApiResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; } = "Request successful.";
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T? Data { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Request successful.")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> FailResponse(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }
    }
}
