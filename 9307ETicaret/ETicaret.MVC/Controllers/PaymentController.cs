using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETicaret.Repository;
using ETicaret.Entity;

namespace ETicaret.MVC.Controllers
{
    public class PaymentController : Controller
    {
        PaymentRepository payRep = new PaymentRepository();
        // GET: Payment

        [HttpGet]
        public ActionResult Pay()
        {
            List<Payment>PaymentTypeList = payRep.List().ProcessResult;
            return View(PaymentTypeList);
        }

        [HttpPost]
        public ActionResult Pay(Invoice model)
        {
            if(model.PaymentTypeID==null)
            //payment type secilmemis.
            //Eğer nullable olmasaydı int değerinin default'u 0 oldugundan if şartının bir de sıfır olmasını kontrol etmemiz gerekir.
            {
                TempData["NoPaymentID"] = "Lütfen ödeme yöntemi seçiniz.";
                return RedirectToAction("Pay");
            }
            Order sepetim = (Order)Session["Order"];
            model.OrderID = sepetim.OrderID;

            InvoiceRepository ip = new InvoiceRepository();
            if (ip.Insert(model).IsSucceeded)
            {
                OrderRepository or = new OrderRepository();
                sepetim.isPay = true;
                or.Update(sepetim);

                Session["OrderSum"] = Session["Order"];

                Session["Order"] = null;
                var sepet = Session["Order"];

                //return RedirectToAction("Index", "Home", new { @paymentMesaj = "Ödeme başarılı" });
                return RedirectToAction("OrderSum", new { @paymentMesaj = "Ödeme başarılı" });
            }
            else
            {
                TempData["PaymentError"] = "Bir şeyler ters gitti";
                //başka bir actiona yönlendirdiğimiz için viewbag ölür. tempdata kulalnıyoruz.
                return RedirectToAction("Pay");
            }
        }
        public ActionResult OrderSum(string paymentMesaj) {
            ViewBag.paymentMesaj = paymentMesaj;
            Order sepettekiler = (Order)Session["OrderSum"];
            ProductRepository pr = new ProductRepository();

            pr.StockAzalt(sepettekiler);

            return View(sepettekiler.OrderDetails);
        }

    }
}