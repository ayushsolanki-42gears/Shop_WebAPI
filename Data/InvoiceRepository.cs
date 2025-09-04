using System.Data;
using Microsoft.Data.SqlClient;
using MyWebApiApp.Models.DTOs;
using MyWebApiApp.Utilities;

namespace MyWebApiApp.Data
{
    public class InvoiceRepository
    {
        private readonly DBHelper _dBHelper;

        public InvoiceRepository(DBHelper dBHelper)
        {
            _dBHelper = dBHelper;
        }

        #region Create Invoice from Cart
        public bool CreateInvoiceFromCart(int? userId)
        {
            int rowAffected = _dBHelper.ExecuteNonQuery(
                "PR_Invoice_Add",
                new SqlParameter("@UserID", userId)
            );
            return rowAffected > 0;
        }
        #endregion

        #region List All Invoices
        public IEnumerable<InvoiceResponse> GetAllInvoices(int? userId)
        {
            var invoices = new List<InvoiceResponse>();
            var dt = _dBHelper.ExecuteDataTable(
                "PR_Invoice_ListAll",
                new SqlParameter("@UserID", userId)
            );

            foreach (DataRow row in dt.Rows)
            {
                invoices.Add(new InvoiceResponse()
                {
                    InvoiceID = row.Field<int>("InvoiceID"),
                    TotalAmount = row.Field<decimal>("TotalAmount"),
                    UserName = row.Field<string>("UserName") ?? string.Empty,
                    Email = row.Field<string>("Email") ?? string.Empty,
                    CreatedDate = row.Field<DateTime>("CreatedDate"),
                    InvoiceItemCount = row.Field<int>("InvoiceItemCount"),
                });
            }
            return invoices;
        }
        #endregion

        #region Get Invoice by ID
        public InvoiceResponse? GetInvoiceByID(int invoiceId)
        {
            var dt = _dBHelper.ExecuteDataTable(
                "PR_Invoice_ListAll",
                new SqlParameter("@InvoiceID", invoiceId)
            );

            var row = dt.AsEnumerable().FirstOrDefault();
            if (row == null) return null;

            return new InvoiceResponse()
            {
                InvoiceID = row.Field<int>("InvoiceID"),
                TotalAmount = row.Field<decimal>("TotalAmount"),
                UserName = row.Field<string>("UserName") ?? string.Empty,
                Email = row.Field<string>("Email") ?? string.Empty,
                CreatedDate = row.Field<DateTime>("CreatedDate"),
                InvoiceItemCount = row.Field<int>("InvoiceItemCount"),
            };
        }
        #endregion

    }
}
