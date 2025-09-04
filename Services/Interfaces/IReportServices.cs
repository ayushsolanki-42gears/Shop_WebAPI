using MyWebApiApp.Models;
using MyWebApiApp.Models.DTOs;

namespace MyWebApiApp.Services.Interfaces
{
    public interface IReportService
    {
        IEnumerable<ReportModel> GetAllReports();
        Task<ReportResponse> CreateReportAsync(ReportRequest request);
        Task<ReportResponse?> GetReportAsync(Guid reportId);
        Task<bool> DeleteReport(string s3Key);
    }
}
