using System.Data;
using Microsoft.Data.SqlClient;
using MyWebApiApp.Models;
using MyWebApiApp.Utilities;

namespace MyWebApiApp.Data
{
    public class InvoiceItemRepository
    {
        private readonly DBHelper _dBHelper;

        public InvoiceItemRepository(DBHelper dBHelper)
        {
            _dBHelper = dBHelper;
        }

        public IEnumerable<InvoiceItemModel> GetAll(int invoiceId)
        {
            var invoiceItems = new List<InvoiceItemModel>();
            var dt = _dBHelper.ExecuteDataTable(
                "PR_InvoiceItem_ListByInvoiceID",
                new SqlParameter("@InvoiceID", invoiceId)
            );
            foreach (DataRow row in dt.Rows)
            {
                invoiceItems.Add(new InvoiceItemModel()
                {
                    ProductName = row["ProductName"].ToString(),
                    Price = Convert.ToDecimal(row["Price"]),
                    Quantity = Convert.ToInt32(row["Quantity"]),
                    TotalAmount = Convert.ToInt32(row["TotalAmount"])
                });
            }
            return invoiceItems;
        }
    }
}