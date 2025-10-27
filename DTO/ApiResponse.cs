namespace AdvanceAPI.DTO
{
    public class ApiResponse
    {
        public int? Status { get; set; }
        public string? Message { get; set; }
        public object? data { get; set; }

        public ApiResponse(int? statusCode, string? message)
        {
            Status = statusCode;
            Message = message;
            data = null;
        }

        public ApiResponse(int? statusCode, string? message, object? data)
        {
            Status = statusCode;
            Message = message;
            this.data = data;
        }


    }
}
