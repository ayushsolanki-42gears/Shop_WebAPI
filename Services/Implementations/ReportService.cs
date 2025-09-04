using Amazon.S3;
using Amazon.S3.Model;
using MyWebApiApp.Data;
using MyWebApiApp.Kafka;
using MyWebApiApp.Models;
using MyWebApiApp.Models.DTOs;
using MyWebApiApp.Services.Interfaces;

namespace MyWebApiApp.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly ReportRepository _reportRepository;
        private readonly KafkaProducer _kafkaProducer;
        private readonly IConfiguration _config;
        private readonly IAmazonS3 _s3;
        private readonly string _bucketName;

        public ReportService(ReportRepository reportRepository, KafkaProducer kafkaProducer, IConfiguration config, IAmazonS3 s3, string bucketName)
        {
            _reportRepository = reportRepository;
            _kafkaProducer = kafkaProducer;
            _config = config;
            _s3 = s3;
            _bucketName = bucketName;
        }

        public IEnumerable<ReportModel> GetAllReports()
        {
            Console.WriteLine("Service");
            var reports = _reportRepository.GetAllReports();
            return reports;
        }


        public async Task<ReportResponse> CreateReportAsync(ReportRequest request)
        {
            var report = new ReportModel
            {
                UserId = request.UserId,
                ReportType = request.ReportType,
                Parameters = string.Join(",", request.Events), // store events as CSV
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            Console.WriteLine("Before");
            Console.WriteLine(request.FromDate);
            Console.WriteLine(request.ToDate);

            if (request.FromDate == null)
            {
                request.FromDate = DateTime.MinValue;
            }

            if (request.ToDate == null)
            {
                request.ToDate = DateTime.UtcNow;
            }

            Console.WriteLine("After");
            Console.WriteLine(request.FromDate);
            Console.WriteLine(request.ToDate);
            var reportId = await _reportRepository.AddReportAsync(report);

            // publish to Kafka so Report Microservice can process
            var topic = _config["Kafka:ReportRequestTopic"] ?? "report-requests";
            await _kafkaProducer.PublishAsync(topic, new
            {
                ReportId = reportId,
                request.UserId,
                request.ReportType,
                request.Events,
                From = request.FromDate,
                To = request.ToDate
            });

            return new ReportResponse
            {
                ReportId = reportId,
                Status = "Pending",
                S3Key = null
            };
        }

        public async Task<ReportResponse?> GetReportAsync(Guid reportId)
        {
            var report = await _reportRepository.GetReportAsync(reportId);
            if (report == null) return null;

            return new ReportResponse
            {
                ReportId = report.ReportId,
                Status = report.Status,
                DownloadUrl = string.IsNullOrEmpty(report.S3Key) ? null : GeneratePresignedUrl(report.S3Key)
            };
        }

        public async Task<bool> DeleteReport(string s3Key)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = s3Key
            };

            var response = await _s3.DeleteObjectAsync(request);
            if (response != null)
            {
                return await _reportRepository.DeleteReportByS3Key(s3Key);
            }
            return false;
        }

        // TODO: replace with real AWS S3 presigned URL generation
        private string GeneratePresignedUrl(string s3Key)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = s3Key,
                Expires = DateTime.UtcNow.AddMinutes(10),
                Verb = HttpVerb.GET
            };
            return _s3.GetPreSignedURL(request);
        }
    }
}
