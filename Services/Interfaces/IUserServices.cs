using MyWebApiApp.Models;
using MyWebApiApp.Models.DTOs;

namespace MyWebApiApp.Services.Interfaces
{
    public interface IUserService
    {
        LoginResponse? Login(string UserName, string Password);
        IEnumerable<UserModel>? GetAllUser();
        void Logout();
    }
}