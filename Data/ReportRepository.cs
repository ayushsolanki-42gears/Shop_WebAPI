using Microsoft.Data.SqlClient;
using MyWebApiApp.Utilities;
using MyWebApiApp.Models;
using System.Data;

namespace MyWebApiApp.Data
{
    public class ReportRepository
    {
        private readonly DBHelper _dBHelper;

        public ReportRepository(DBHelper dBHelper)
        {
            _dBHelper = dBHelper;
        }

        #region Get All Reports
        public IEnumerable<ReportModel> GetAllReports()
        {
            Console.WriteLine("Repo");
            var dt = _dBHelper.ExecuteDataTable(
                "PR_Report_SelectAll"
            );

            var reports = new List<ReportModel>();

            foreach (DataRow row in dt.Rows)
            {
                reports.Add(
                    new ReportModel()
                    {
                        Parameters = row.Field<string>("Parameters") ?? string.Empty,
                        ReportId = row.Field<Guid>("ReportId"),
                        CreatedAt = row.Field<DateTime>("CreatedAt"),
                        Status = row.Field<string>("Status") ?? string.Empty,
                    }
                );
            }

            return reports;
        }
        #endregion

        #region Add Report
        public async Task<Guid> AddReportAsync(ReportModel report)
        {
            var reportId = Guid.NewGuid();

            await _dBHelper.ExecuteNonQueryAsync(
                "PR_Report_Add",
                new SqlParameter("@ReportId", reportId),
                new SqlParameter("@UserId", report.UserId),
                new SqlParameter("@ReportType", report.ReportType),
                new SqlParameter("@Parameters", (object?)report.Parameters ?? DBNull.Value),
                new SqlParameter("@Status", report.Status),
                new SqlParameter("@CreatedAt", report.CreatedAt)
            );

            return reportId;
        }
        #endregion

        #region Get Report
        public async Task<ReportModel?> GetReportAsync(Guid reportId)
        {
            var dt = await _dBHelper.ExecuteDataTableAsync(
                "PR_Report_Get",
                new SqlParameter("@ReportId", reportId)
            );

            if (dt.Rows.Count == 0)
                return null;

            var row = dt.Rows[0];
            return new ReportModel
            {
                ReportId = (Guid)row["ReportId"],
                UserId = Convert.ToInt32(row["UserId"]),
                ReportType = row["ReportType"].ToString()!,
                Parameters = row["Parameters"] == DBNull.Value ? null : row["Parameters"].ToString(),
                Status = row["Status"].ToString()!,
                S3Key = row["S3Key"] == DBNull.Value ? null : row["S3Key"].ToString(),
                CreatedAt = Convert.ToDateTime(row["CreatedAt"]),
                CompletedAt = row["CompletedAt"] == DBNull.Value ? null : Convert.ToDateTime(row["CompletedAt"])
            };
        }
        #endregion

        #region Update Report
        public async Task<bool> UpdateReportAsync(ReportModel report)
        {
            int rowsAffected = await _dBHelper.ExecuteNonQueryAsync(
                "PR_Report_Update",
                new SqlParameter("@ReportId", report.ReportId),
                new SqlParameter("@Status", report.Status),
                new SqlParameter("@S3Key", (object?)report.S3Key ?? DBNull.Value),
                new SqlParameter("@CompletedAt", (object?)report.CompletedAt ?? DBNull.Value)
            );

            return rowsAffected > 0;
        }
        #endregion

        public async Task<bool> DeleteReportByS3Key(string s3key)
        {
            int rowsAffected = await _dBHelper.ExecuteNonQueryAsync(
                "PR_Report_Delete",
                new SqlParameter("@S3Key", s3key)
            );

            return rowsAffected > 0;
        }
    }
}
