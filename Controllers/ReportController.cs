using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.Models.DTOs;
using MyWebApiApp.Services.Interfaces;

namespace MyWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public IActionResult GetAllReports()
        {
            Console.WriteLine("Report Controller");
            var reports = _reportService.GetAllReports();
            if (reports == null || !reports.Any())
            {
                return NotFound(new ApiResponse("Reports not found", 404));
            }
            return Ok(new ApiResponse(reports, "Reports fetch successfully", 200));
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] ReportRequest request)
        {
            if (request == null) return BadRequest("Invalid report request");

            int? userId = HttpContext.Session.GetInt32("UserID");
            request.UserId = userId ?? 0;
            var response = await _reportService.CreateReportAsync(request);
            return Ok(new ApiResponse(response, "Report Create Successfully", 200));
        }

        [HttpGet("{reportId:guid}")]
        public async Task<IActionResult> GetReport(Guid reportId)
        {
            if (reportId == Guid.Empty) throw new ArgumentException("Invalid ReportID");

            var response = await _reportService.GetReportAsync(reportId);
            if (response == null) return NotFound(new ApiResponse("Report not found", 404));

            return Ok(new ApiResponse(response, "Report Fetch Successfully", 200));
        }

        [HttpDelete("{*s3Key}")]
        public async Task<IActionResult> DeleteReport(string s3Key)
        {
            if (string.IsNullOrWhiteSpace(s3Key))
            {
                throw new ArgumentException("Invalid s3key");
            }

            bool isDeleted = await _reportService.DeleteReport(s3Key);
            if (!isDeleted) throw new Exception("Error while deleting report");
            return Ok(new ApiResponse("Delete successful", 200));
        }
    }
}
