namespace LibraryManagement.Application.DTOs.Response
{
    public class ApiResponse <T>
    {
        public int Code { get; set; } = 1000;
        public string? Status { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse() { }

        // success
        public ApiResponse(T? data)
        {
            Status = "Success";
            Message = null;
            Data = data;
        }

        // fail
        public ApiResponse(int code, string message)
        {
            Status = "Fail";
            Code = code;
            Message = message;
            Data = default;
        }

        public static ApiResponse<T> Success(T? data) => new ApiResponse<T>(data);

        public static ApiResponse<T> Fail(int code, string message) => new ApiResponse<T>(code, message);
    }
}
