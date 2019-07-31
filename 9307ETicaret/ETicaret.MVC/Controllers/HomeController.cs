using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETicaret.Repository;
using ETicaret.Entity;

namespace ETicaret.MVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        ProductRepository pr = new ProductRepository();

        public ActionResult Index(string paymentMesaj) //anasayfa
        {
            ViewBag.payment = paymentMesaj;
            return View(pr.GetLatestObjects(5).ProcessResult);
        }

        //Soldaki kategori menüsündeki kategori isimlerine tıklağımızda o kategorideki ürünleri getiren method.
        public ActionResult GetProductByCatID(Guid id)
        {
            List<Product> ProList = ProductRepository.db.Products.Where(t => t.CategoryID == id).ToList();

            //var PList = pr.List().ProcessResult.Where(t => t.CategoryID == id).ToList();
            //2. yöntemde bütün productlar çekildikten sonra filtreleme yapılıyor. Bu sebeple işlemi daha yorucu.

            return View(ProList);
        }

        public ActionResult GetProductByBrandID(int id)
        {
            List<Product> ProList = ProductRepository.db.Products.Where(t => t.BrandID == id).ToList();
            return View(ProList);
        }

        public ActionResult GetAllProducts()
        {
            List<Product> ProList = ProductRepository.db.Products.ToList();
            return View(ProList);

            //yukarıdaki productrep'ten oluşturulan instance direk view methodunun icinde kullanılarak da yapabiliriz.
            //return View(pr.List().ProcessResult);
        }

        public ActionResult Detail(int id)
        {
            return View(pr.GetObjByID(id).ProcessResult);
        }
    }
}