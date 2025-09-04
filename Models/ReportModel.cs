namespace MyWebApiApp.Models;

public class ReportModel
{
    public Guid ReportId { get; set; }
    public int UserId { get; set; }
    public string ReportType { get; set; } = "Logs";
    public string Parameters { get; set; } = string.Empty; // store JSON filters
    public string Status { get; set; } = "Pending"; // Pending, Completed, Failed
    public string? S3Key { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}
