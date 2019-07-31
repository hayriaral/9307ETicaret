using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETicaret.Repository;
using ETicaret.Entity;

namespace ETicaret.MVC.Controllers
{
    public class OrderController : Controller
    {
        OrderRepository or = new OrderRepository();
        OrderDetailRepository odr = new OrderDetailRepository();
        ProductRepository pr = new ProductRepository();
        // GET: Order
        public ActionResult AddOrder(int id)
            //Bu method ürünlerin altındaki alışveriş arabası butonu ile tetiklenecek.
        {
            //Sepet oluşturacağız, sepeti de session üzerinde tutacağız. Order için yaratacağımız session Session["Order"] diye çağıracağız.
            if (Session["Order"]==null)
                //Sepet oluşturulmamışsa
            {
                Order newOrder = new Order();
                newOrder.isPay = false;
                or.Insert(newOrder);

                Order dbdeki = or.GetLatestObjects(1).ProcessResult.SingleOrDefault();
                Session["Order"] = dbdeki;

                OrderDetail newOD = new OrderDetail();
                newOD.OrderID = ((Order)Session["Order"]).OrderID;
                newOD.ProductID = id;
                newOD.Quantitiy = 1;
                newOD.Price = pr.GetObjByID(id).ProcessResult.Price;
                odr.Insert(newOD);
            }
            else
            //sepet oluşturulmuş ve aynı sepete ürün eklenmek isteniyor.
            {
                Order sepetteki = (Order)Session["Order"];
                OrderDetail sepettekiOrd = odr.GetObjectByTwoID(sepetteki.OrderID, id).ProcessResult;
                //Daha önce sepete eklenmis bir ürün aynı sepete tekrar eklenmek istenirse, bu ürünün quantity'sini arttırmamız gerekiyor. O seneple bu üründen sepette mevcut mu diye bakmalıyız.

                if (sepettekiOrd==null)
                    //demektir ki sepete yeni bir ürün ekleniyor.
                {
                    OrderDetail yeniProductOrd = new OrderDetail();
                    yeniProductOrd.OrderID = sepetteki.OrderID;
                    yeniProductOrd.ProductID = id;
                    yeniProductOrd.Price = pr.GetObjByID(id).ProcessResult.Price;
                    yeniProductOrd.Quantitiy = 1;
                    odr.Insert(yeniProductOrd);
                }
                else
                {
                    sepettekiOrd.Quantitiy++;
                    sepettekiOrd.Price += pr.GetObjByID(id).ProcessResult.Price;
                    odr.Update(sepettekiOrd);
                }
            }
            return RedirectToAction("Index","Home");
        }

        public ActionResult DetailPage()
        {
            if (Session["Order"]!=null)
            {
                Order sepetim = (Order)Session["Order"];
                decimal? totalPrice = 0;
                foreach(OrderDetail item in sepetim.OrderDetails)
                {
                    totalPrice += item.Price;
                }
                sepetim.TotalPrice = totalPrice;
                or.Update(sepetim);

                return View(sepetim.OrderDetails);
            }
            else
            {
                return RedirectToAction("Index", "Home");
                //Bir order yoksa alışveriş butonuna basıldığında bi işlem olmayacaktır.
            }
        }

        public ActionResult QuantityArttir(int id)
        {
            Order sepetim = (Order)Session["Order"];
            OrderDetail sepettekiOrd = odr.GetObjectByTwoID(sepetim.OrderID, id).ProcessResult;

            sepettekiOrd.Quantitiy++;
            sepettekiOrd.Price += pr.GetObjByID(id).ProcessResult.Price;
            odr.Update(sepettekiOrd);
            return RedirectToAction("DetailPage");
        }

        public ActionResult QuantityAzalt(int id)
        {
            Order sepetim = (Order)Session["Order"];
            OrderDetail sepettekiOrd = odr.GetObjectByTwoID(sepetim.OrderID, id).ProcessResult;

            if (sepettekiOrd.Quantitiy>1)
            {
                sepettekiOrd.Quantitiy--;
                sepettekiOrd.Price -= pr.GetObjByID(id).ProcessResult.Price;
                odr.Update(sepettekiOrd);
                return RedirectToAction("DetailPage");
            }
            else
            {
                return RedirectToAction("Delete", new { @ID = id });
            }
        }

        public ActionResult Delete(int id)
        {
            Order sepetim = (Order)Session["Order"];
            odr.DeleteObjects(sepetim.OrderID, id);

            return RedirectToAction("DetailPage");
        }
    }
}