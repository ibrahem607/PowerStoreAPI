using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Contract.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public int StatusCode { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Operation completed successfully", int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
        }

        public static ApiResponse<T> ErrorResponse(string message, List<string>? errors = null, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors,
                StatusCode = statusCode
            };
        }
    }

    // For void operations
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string>? Errors { get; set; }
        public int StatusCode { get; set; }

        public static ApiResponse SuccessResponse(string message = "Operation completed successfully", int statusCode = 200)
        {
            return new ApiResponse
            {
                Success = true,
                Message = message,
                StatusCode = statusCode
            };
        }

        public static ApiResponse ErrorResponse(string message, List<string>? errors = null, int statusCode = 400)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message,
                Errors = errors,
                StatusCode = statusCode
            };
        }
    }
}
