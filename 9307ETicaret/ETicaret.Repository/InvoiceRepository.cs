using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Entity;
using ETicaret.Common;

namespace ETicaret.Repository
{
    public class InvoiceRepository
    {
        public static ECommerceEntities db = Tool.GetConnection();

        ResultProcess<Invoice> Result = new ResultProcess<Invoice>();

        public Result<int> Insert(Invoice item)
        {
            Invoice newInvoice = db.Invoices.Create();

            newInvoice.Address = item.Address;
            newInvoice.OrderID = item.OrderID;
            newInvoice.PaymentDate = DateTime.Now;
            newInvoice.PaymentTypeID = item.PaymentTypeID;

            db.Invoices.Add(newInvoice);

            return Result.GetResult(db);
        }
    }
}
