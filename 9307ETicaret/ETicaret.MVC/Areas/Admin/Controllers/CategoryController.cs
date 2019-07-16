using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETicaret.Entity;
using ETicaret.Repository;
using ETicaret.MVC.Areas.Admin.Models.ResultModel;

namespace ETicaret.MVC.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        CategoryRepository cr = new CategoryRepository();

        //Result<int> resultint = new Result<int>();
        //Result<Category> resultT = new Result<Category>();
        //Result<List<Category>> resultList = = new Result<List<Category>>();

        //Yukarıdaki 3 i stace'ı kullanmak yerine bunları InstanceResult classının propertyleri haline getirdik.
        InstanceResult<Category> result = new InstanceResult<Category>();
        
        // GET: Admin/Category
        public ActionResult List(string mesaj)
        {
            result.ResultList = cr.List();
            ViewBag.silmeMesajı = mesaj;
            return View(result.ResultList.ProcessResult);
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(Category model)
        {
            result.ResultInt = cr.Insert(model);
            ViewBag.basarili = result.ResultInt.UserMessage;
            return View();
        }

        [HttpGet]
        public ActionResult EditCategory(Guid id)
        {
            result.ResultT = cr.GetObjByID(id);
            return View(result.ResultT.ProcessResult);
        }

        [HttpPost]
        public ActionResult EditCategory(Category model)
        {
            result.ResultInt = cr.Update(model);
            ViewBag.mesaj = result.ResultInt.UserMessage;
            return View(model);
        }

        public ActionResult DeleteCategory(Guid id)
        {
            result.ResultInt = cr.Delete(id);
            return RedirectToAction("List", new { @mesaj = result.ResultInt.UserMessage });
            //parametrenin gidebilmesi için yukarıdaki List methoduan parametre ekledik.
        }
    }
}