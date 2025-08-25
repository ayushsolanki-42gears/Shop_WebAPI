// using Microsoft.AspNetCore.Diagnostics;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Data.SqlClient;
// using MyWebApiApp.Models.DTOs;

// namespace MyWebApiApp.Controllers
// {
//     [ApiController]
//     public class ErrorController : ControllerBase
//     {
//         [Route("/error")]
//         public IActionResult HandleError()
//         {
//             Console.BackgroundColor = ConsoleColor.Black;
//             var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
//             var exception = context?.Error;

//             if (exception is SqlException sqlEx)
//             {
//                 // You can inspect sqlEx.Number or Message
//                 return StatusCode(StatusCodes.Status400BadRequest,
//                     new ApiResponse(sqlEx.Message, StatusCodes.Status400BadRequest));
//             }
//             else if (exception is UnauthorizedAccessException)
//             {
//                 return StatusCode(StatusCodes.Status401Unauthorized,
//                     new ApiResponse("Unauthorized access", StatusCodes.Status401Unauthorized));
//             }
//             else if (exception is ArgumentException argEx)
//             {
//                 return StatusCode(StatusCodes.Status400BadRequest,
//                     new ApiResponse(argEx.Message, StatusCodes.Status400BadRequest));
//             }

//             // fallback
//             return StatusCode(StatusCodes.Status500InternalServerError,
//                 new ApiResponse("Internal Server Error", StatusCodes.Status500InternalServerError));
//         }

//     }
// }