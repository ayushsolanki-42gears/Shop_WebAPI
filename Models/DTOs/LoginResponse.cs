namespace MyWebApiApp.Models.DTOs
{
    public class LoginResponse
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}