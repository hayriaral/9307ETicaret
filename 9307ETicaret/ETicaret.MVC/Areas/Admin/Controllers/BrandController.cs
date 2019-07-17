using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETicaret.Repository;
using ETicaret.Entity;
using ETicaret.MVC.Areas.Admin.Models.ResultModel;

namespace ETicaret.MVC.Areas.Admin.Controllers
{
    public class BrandController : Controller
    {
        BrandRepository br = new BrandRepository();
        InstanceResult<Brand> result = new InstanceResult<Brand>();
        // GET: Admin/Brand
        public ActionResult BrandList(string mesaj)
        {
            result.ResultList = br.List();
            if (mesaj != null)
            {
                ViewBag.deleteMessage = String.Format("Silme işlemi{0}", mesaj);
            }
            
            return View(result.ResultList.ProcessResult);
        }
        [HttpGet]
        public ActionResult AddBrand()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddBrand(Brand model, HttpPostedFileBase file)
        //gönderdiğimiz file 2. parametreye düşecek.
        {
            string PhotoName = ""; //Resmin adı
            if (file != null && file.ContentLength > 0)
            {
                PhotoName = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                //replace string methodudur o yüzden araya tostring ekledik.
                string path = Server.MapPath("~/Images/" + PhotoName); //resmin kaydolacağı yer.
                file.SaveAs(path); //resmi belirtilen path'e kaydettik.
            }
            model.Photo = PhotoName;
            //modele path'i değil, photo name verilir. bunun sebebi path'de bir değişiklik olduğunda işlemin zorlaşmaması.
            result.ResultInt = br.Insert(model);
            if (result.ResultInt.IsSucceeded)
            {
                return RedirectToAction("BrandList");
            }
            else
            {
                ViewBag.hataMesaji = result.ResultInt.UserMessage;
                return RedirectToAction("AddBrand");
            }
        }

        [HttpGet]
        public ActionResult EditBrand(int id)
        {
            result.ResultT = br.GetObjByID(id);
            return View(result.ResultT.ProcessResult);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult EditBrand(Brand model, HttpPostedFileBase Photo)
        {
            string PhotoName = model.Photo; //Model'in photosonu hidden input olarak göndermezsek Photo propert'si null olacaktır. Bu da kullanıcı fotoğrafı değiştirmek istemezse hataya sebep olacaktır.

            if(Photo != null && Photo.ContentLength > 0)
            {
                PhotoName = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                string path = Server.MapPath("~/Images" + PhotoName);
                Photo.SaveAs(path);
            }

            model.Photo = PhotoName;
            result.ResultInt = br.Update(model);
            if (result.ResultInt.IsSucceeded)
            {
                return RedirectToAction("BrandList");
            }
            else
            {
                return RedirectToAction("EditBrand", new { @id = model.BrandID });
                //return View();
            }
        }

        public ActionResult DeleteBrand(int id)
        {
            result.ResultInt = br.Delete(id);
            return RedirectToAction("BrandList", new { @mesaj = result.ResultInt.UserMessage });
        }
    }
}