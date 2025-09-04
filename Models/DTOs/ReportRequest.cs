namespace MyWebApiApp.Models.DTOs
{
    public class ReportRequest
    {
        public int UserId { get; set; }
        public string ReportType { get; set; } = string.Empty;
        public List<string> Events { get; set; } = new();
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}