using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETicaret.Entity;
using ETicaret.Repository;
using ETicaret.MVC.Areas.Admin.Models.ResultModel;
using ETicaret.MVC.Areas.Admin.Models.ProductVM;

namespace ETicaret.MVC.Areas.Admin.Controllers {
    public class ProductController : Controller {
        ProductRepository pr = new ProductRepository();
        InstanceResult<Product> result = new InstanceResult<Product>();
        // GET: Admin/Product
        public ActionResult ProductList() {
            result.ResultList = pr.List();
            return View(result.ResultList.ProcessResult);
        }

        //Product ekleme sayfasını açarken, sistemde mevcut olan Category ve Brand'leri getirmeliyiz ki Admin bunlar arasından seçim yapabilsin.
        //CatList ve BrandList'i tek bir modelde toplamak için bir ViewModel class'ı olan ProductViewModel'ı yaratıyoruz.
        [HttpGet]
        public ActionResult AddProduct() {
            CategoryRepository cr = new CategoryRepository();
            BrandRepository br = new BrandRepository();

            ProductViewModel pwm = new ProductViewModel();

            pwm.CatList = cr.List().ProcessResult;
            //Esas data ProcessResult'da tutuluyor.
            pwm.BrandList = br.List().ProcessResult;

            return View(pwm);
        }
        [HttpPost]
        public ActionResult AddProduct(Product model, HttpPostedFileBase Photo, HttpPostedFileBase Photo2, HttpPostedFileBase Photo3) {
            string PhotoName = "";
            string PhotoName2 = "";
            string PhotoName3 = "";

            if (Photo != null && Photo.ContentLength > 0) {
                PhotoName = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                string path = Server.MapPath("~/Images/" + PhotoName);
                Photo.SaveAs(path);
            }

            if (Photo2 != null && Photo2.ContentLength > 0) {
                PhotoName2 = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                string path = Server.MapPath("~/Images/" + PhotoName2);
                Photo2.SaveAs(path);
            }

            if (Photo3 != null && Photo3.ContentLength > 0) {
                PhotoName3 = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                string path = Server.MapPath("~/Images/" + PhotoName3);
                Photo3.SaveAs(path);
            }

            //Foto ekleme gibi tekrar eden işlemleri fonksiyın veya method haline getirmek için. MVC katmanında Admin altına açılacak "HELPER" isimli klasörde yeni bir class yaratarak gerekli işlemler için fonksiyonlar yaratılabilir.
            model.Photo = PhotoName;
            model.Photo2 = PhotoName2;
            model.Photo3 = PhotoName3;

            result.ResultInt = pr.Insert(model);

            if (result.ResultInt.IsSucceeded) {
                return RedirectToAction("ProductList");
            }
            return RedirectToAction("AddProduct");
        }

        //Pwm'yi güncelledik. Çünkü burada listelerin yanısıra Product objesi de gördermemiz gerekmekte.
        [HttpGet]
        public ActionResult EditProduct(int id) {
            CategoryRepository cr = new CategoryRepository();
            BrandRepository br = new BrandRepository();
            ProductViewModel pwm = new ProductViewModel();

            pwm.CatList = cr.List().ProcessResult;
            pwm.BrandList = br.List().ProcessResult;
            pwm.Product = pr.GetObjByID(id).ProcessResult;

            return View(pwm);
        }
        [HttpPost]
        public ActionResult EditProduct(Product model, HttpPostedFileBase Photo, HttpPostedFileBase Photo2, HttpPostedFileBase Photo3) {
            string PhotoName = model.Photo;
            string PhotoName2 = model.Photo2;
            string PhotoName3 = model.Photo3;

            if (Photo != null && Photo.ContentLength > 0) {
                PhotoName = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                //if (Photo.ContentType == "image/png") {
                //    PhotoName += "png";
                //}
                //else if (Photo.ContentType == "image/jpg") {
                //    PhotoName += ".jpg";
                //}
                //else if (Photo.ContentType == "image/bmp") {
                //    PhotoName += ".bmp";
                //}
                //else {
                //    TempData["PhotoExtensionError"] = "Lütfen jpg, png ya da bmp formatında resim yükleyiniz";
                //    return RedirectToAction("EditProduct", new { @id = model.ProductID });
                //}

                string path = Server.MapPath("~/Images/" + PhotoName);
                Photo.SaveAs(path);
            }

            if (Photo2 != null && Photo2.ContentLength > 0) {
                PhotoName2 = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                string path = Server.MapPath("~/Images/" + PhotoName2);
                Photo2.SaveAs(path);
            }

            if (Photo3 != null && Photo3.ContentLength > 0) {
                PhotoName3 = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                string path = Server.MapPath("~/Images/" + PhotoName3);
                Photo3.SaveAs(path);
            }

            model.Photo = PhotoName;
            model.Photo2 = PhotoName2;
            model.Photo3 = PhotoName3;

            result.ResultInt = pr.Update(model);
            if (result.ResultInt.IsSucceeded) {
                return RedirectToAction("ProductList");
            }
            else {
                return RedirectToAction("EditProduct", new { @id = model.ProductID });
            }
        }
        public ActionResult DeleteProduct(int id) {
            string photoName = pr.GetObjByID(id).ProcessResult.Photo;
            string photoName2 = pr.GetObjByID(id).ProcessResult.Photo2;
            string photoName3 = pr.GetObjByID(id).ProcessResult.Photo3;



            if (photoName != null) {
                string fullPath = Request.MapPath("~/Images/" + photoName);
                System.IO.File.Delete(fullPath);
            }

            if (photoName2 != null) {
                string fullPath2 = Request.MapPath("~/Images/" + photoName2);
                System.IO.File.Delete(fullPath2);
            }

            if (photoName3 != null) {
                string fullPath3 = Request.MapPath("~/Images/" + photoName3);
                System.IO.File.Delete(fullPath3);
            }

            //Üstteki adımda product silinmeden hemen kayıtlı olduğu klasörden resmini de kaldırıyoruz.
            result.ResultInt = pr.Delete(id);
            return RedirectToAction("ProductList");
        }
    }
}