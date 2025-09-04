using MyWebApiApp.Models;
using Microsoft.Data.SqlClient;
using MyWebApiApp.Models.DTOs;
using MyWebApiApp.Utilities;
using System.Data;
namespace MyWebApiApp.Data
{
    public class UserRepository
    {
        private readonly DBHelper _dBHelper;
        public UserRepository(DBHelper dBHelper)
        {
            _dBHelper = dBHelper;
        }

        #region UserList
        public IEnumerable<UserModel> SelectAll()
        {
            var users = new List<UserModel>();
            var dt = _dBHelper.ExecuteDataTable("PR_User_UserList");

            foreach (DataRow row in dt.Rows)
            {
                users.Add(new UserModel()
                {
                    UserID = row.Field<int>("UserID"),
                    UserName = row.Field<string>("UserName") ?? "",
                    Email = row.Field<string>("Email") ?? "",
                    Password = row.Field<string>("Password") ?? ""

                });
            }

            return users;
        }
        #endregion

        #region Login
        public LoginResponse? Login(string userName, string password)
        {
            var dt = _dBHelper.ExecuteDataTable(
                "PR_User_Login",
                new SqlParameter("@UserName", userName),
                new SqlParameter("@Password", password)
            );

            var row = dt.AsEnumerable().FirstOrDefault();
            if (row == null) return null;
            
            return new LoginResponse()
            {
                UserID = row.Field<int>("UserID"),
                UserName = row.Field<string>("UserName") ?? "",
                Role = row.Field<string>("Role") ?? ""
            };
        }
        #endregion
    }
}