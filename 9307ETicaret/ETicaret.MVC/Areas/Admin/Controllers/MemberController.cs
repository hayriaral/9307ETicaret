using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETicaret.Repository;
using ETicaret.MVC.Areas.Admin.Models.ResultModel;
using ETicaret.Entity;

namespace ETicaret.MVC.Areas.Admin.Controllers
{
    public class MemberController : Controller
    {
        // GET: Admin/Member

        MemberRepository mr = new MemberRepository();
        InstanceResult<Member> result = new InstanceResult<Member>();

        public ActionResult MemberList(string message, int? ID)
        {
            //int? nullable olmalıdır çünkü null olmazsa, silme işlemi değil de listeyi açma işlemi yapılıyorsa parametreler null geleektir. stringin null gelmesinde sıkıntı yoktur, ancak int değeri null değeri olamayacağı için null exception fırlatır.
            result.ResultList = mr.List();
            if (message!=null && ID!=null) // bunlar null değilse silme işlemi yapılıyor demektir.
            {
                ViewBag.SilmeMesaji = String.Format("{0} id'li kullanıcının silme işlemi {1}", ID, message);
            }
            return View(result.ResultList.ProcessResult);
        }

        [HttpGet]
        public ActionResult AddMember(string mesajUye, string mesajPassword)
        {
            ViewBag.mesajUye = mesajUye;
            ViewBag.mesajPasswrod = mesajPassword;
            return View();
        }

        [HttpPost]
        public ActionResult AddMember(Member model, string confirm)
        {
            //Bu maille kaydolmuş biri var mı?
            Member yeniUye = MemberRepository.db.Members.SingleOrDefault(t => t.Email == model.Email);
            //Burada bir kullanıcı dönüyorsa demek ki modelde belirtilen maille daha önce biri kayıt olmuş.

            if (yeniUye == null) //modeldeki maille bir üye yok.
            {
                if ((model.Password!=null && model.Password!="") && confirm==model.Password)
                {
                    model.RoleID = 1; //admin olarak ekliyoruz.
                    result.ResultInt = mr.Insert(model); //dbye ekliyoruz.

                    if (result.ResultInt.IsSucceeded)
                    {
                        return RedirectToAction("MemberList");
                    }

                    return RedirectToAction("AddMember");
                   
                }
                else
                {
                    return RedirectToAction("AddMember", new { @mesajPassword = "Parolalar uyuşmuyor" });
                }
            }
            else
            {
                return RedirectToAction("AddMember", new {@mesajUye = "Bu maille kayıtlı bir admin bulunmaktadır" });
            }

        }

        [HttpGet]
        public ActionResult EditMember(int id)
        {
            return View(mr.GetObjByID(id).ProcessResult);
        }

        [HttpPost]
        public ActionResult EditMember(Member model, string confirm, string oldPassword, string newPassword)
        {
            if (model.Password==oldPassword)
            {
                if (newPassword == confirm)
                {
                    model.Password = newPassword;
                    result.ResultInt = mr.Update(model);

                    if (result.ResultInt.IsSucceeded)
                    {
                        return RedirectToAction("MemberList");
                    }

                    return RedirectToAction("EditMember", new { @id = model.UserID });
                }
                else
                {
                    ViewBag.HataPassword = "Parolalar uyuşmuyor.";
                    return View(model);
                }
            }
            else
            {
                ViewBag.OldPasswordWrong = "Eski parolayı yanlış girdiniz.";
                return View(model);
            }

        }

        public ActionResult DeleteMember(int id)
        {
            result.ResultInt = mr.Delete(id);
            return RedirectToAction("MemberList", new { @ID = id, @Message = result.ResultInt.UserMessage });
        }
    }
}