namespace LibraryManagement.DTOs
{
    public class ApiResponse <T>
    {
        public int Code { get; set; } = 1000;
        public string Message { get; set; } = "Success";
        public T? Data { get; set; }

        public ApiResponse() { }

        // success
        public ApiResponse(T? data)
        {
            Data = data;
        }

        // fail
        public ApiResponse(int code)
        {
            Code = code;
            Data = default;
        }

        public static ApiResponse<T> Success(T? data) => new ApiResponse<T>(data);

        public static ApiResponse<T> Fail(int code) => new ApiResponse<T>(code);
    }
}
