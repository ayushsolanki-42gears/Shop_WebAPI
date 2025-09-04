namespace MyWebApiApp.Models;

public class ReportResponse
{
    public Guid ReportId { get; set; }
    public string Status { get; set; } = "Pending";
    public string? S3Key { get; set; }

    public string? DownloadUrl { get; set; }
}
