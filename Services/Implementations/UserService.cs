using MyWebApiApp.Data;
using MyWebApiApp.Models;
using MyWebApiApp.Models.DTOs;
using MyWebApiApp.Services.Interfaces;

namespace MyWebApiApp.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogServices _logServices;
        public UserService(UserRepository userRepository, IHttpContextAccessor httpContextAccessor, ILogServices logServices)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _logServices = logServices;
        }

        #region Login
        public LoginResponse? Login(string userName, string password)
        {
            var user = _userRepository.Login(userName, password);
            if (user != null)
            {
                var session = _httpContextAccessor.HttpContext!.Session;
                session.SetInt32("UserID", user.UserID);
                session.SetString("UserName", user.UserName);
                session.SetString("Role", user.Role);
            }
            return user;
        }
        #endregion

        #region get all user
        public IEnumerable<UserModel> GetAllUser()
        {
            var users = _userRepository?.SelectAll();
            return users ?? Enumerable.Empty<UserModel>();
        }
        #endregion

        #region logout
        public void Logout()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null || !httpContext.Session.Keys.Any())
                return;

            var session = httpContext.Session;
            string? role = session.GetString("Role");
            int? userIdValue = session.GetInt32("UserID");
            string? userName = session.GetString("UserName");
            string? userId = userIdValue?.ToString();

            if (!string.IsNullOrEmpty(userName))
            {
                _logServices.InsertLog("Logout", $"Logout by {userName}.", userId);
            }

            session.Clear();
        }
        #endregion

    }
}