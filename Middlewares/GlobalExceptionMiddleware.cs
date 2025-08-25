using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MyWebApiApp.Models.DTOs;
using System;
using System.Threading.Tasks;

namespace MyWebApiApp.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Continue pipeline
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");

                context.Response.ContentType = "application/json";

                // Decide status code & response message
                ApiResponse response;
                int statusCode;

                switch (ex)
                {
                    case SqlException sqlEx:
                        statusCode = StatusCodes.Status400BadRequest;
                        response = new ApiResponse(sqlEx.Message, statusCode);
                        break;

                    case UnauthorizedAccessException:
                        statusCode = StatusCodes.Status401Unauthorized;
                        response = new ApiResponse("Unauthorized access", statusCode);
                        break;

                    case ArgumentException argEx:
                        statusCode = StatusCodes.Status400BadRequest;
                        response = new ApiResponse(argEx.Message, statusCode);
                        break;

                    default:
                        statusCode = StatusCodes.Status500InternalServerError;
                        response = new ApiResponse("Internal Server Error", statusCode);
                        break;
                }

                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
