using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.Filters;
using MyWebApiApp.Models.DTOs;
using MyWebApiApp.Services.Interfaces;

namespace MyWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet()]
        #region Get All User
        public IActionResult GetAllUsers()
        {
            ApiResponse response;
            var users = _userService?.GetAllUser();
            if (users == null || !users.Any())
            {
                response = new ApiResponse("Users not found", 404);
                return NotFound(response);
            }
            response = new ApiResponse(users, "Users fetch successfully", 200);
            return Ok(response);
        }
        #endregion

        [LogAction("Login")]
        [HttpPost("LoginUser")]
        #region LoginUser
        public IActionResult LoginUser(LoginRequest? request)
        {
            ApiResponse response;
            if (request == null || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                throw new ArgumentException("Username or Password is not provided");
            }
            var user = _userService?.Login(request.UserName, request.Password);
            if (user == null)
            {
                response = new ApiResponse("Invalid Credential", 404);
                return NotFound(response);
            }
            response = new ApiResponse(user, "User login successfully", 200);
            return Ok(response);
        }
        #endregion

        [HttpPost("Logout")]
        #region Logout user
        public IActionResult Logout()
        {
            _userService.Logout();
            ApiResponse response = new ApiResponse("Logged out successfully.", 200);
            return Ok(response);
        }
        #endregion
    }
}