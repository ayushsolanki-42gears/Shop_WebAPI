namespace MyWebApiApp.Models.DTOs
{
    public class ApiResponse
    {
        public bool success { get; set; }
        public dynamic? data { get; set; }
        public string message { get; set; }
        public int statusCode { get; set; }

        public ApiResponse(string message, int statusCode) { 
            this.message = message;
            this.statusCode = statusCode;
            this.success = statusCode < 400;
        }
        public ApiResponse(dynamic data, string message, int statusCode) { 
            this.data = data;
            this.message = message;
            this.statusCode = statusCode;
            this.success = statusCode < 400;
        }
    }
}