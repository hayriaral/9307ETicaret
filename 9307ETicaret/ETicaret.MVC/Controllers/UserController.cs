using ETicaret.Entity;
using ETicaret.MVC.Models.ResultModel;
using ETicaret.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETicaret.MVC.Controllers {
    public class UserController : Controller {
        MemberRepository mr = new MemberRepository();
        UserInstanceResult<Member> result = new UserInstanceResult<Member>();

        OrderDetailRepository odr = new OrderDetailRepository();
        // GET: User
        [HttpGet]
        public ActionResult SignUp(string mesajUye, string mesajPassword) {
            ViewBag.mesajUye = mesajUye;
            ViewBag.mesajPassword = mesajPassword;
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(Member model, string confirm) {
            //Bu maille kaydolmuş biri var mı?
            Member newUser = MemberRepository.db.Members.SingleOrDefault(t => t.Email == model.Email);
            //Burada bir kullanıcı dönüyorsa demek ki modelde belirtilen maille daha önce biri kayıt olmuş.

            if (newUser == null) {
                if ((model.Password != null && model.Password != "") && confirm == model.Password) {
                    model.RoleID = 2; //kullanıcı olarak ekliyoruz.
                    result.ResultInt = mr.Insert(model); //dbye ekliyoruz.

                    if (result.ResultInt.IsSucceeded) {
                        return RedirectToAction("Index", "Home", new { @yeniUyeMesaj = "Üye olma işlemi başarılı!" });
                    }

                    return RedirectToAction("SignUp");

                }
                else {
                    return RedirectToAction("SignUp", new { @mesajPassword = "Parolalar uyuşmuyor" });
                }
            }
            else {
                return RedirectToAction("SignUp", new { @mesajUye = "Bu maille kayıtlı başka bir kullanıcı bulunmaktadır" });
            }
        }
        [HttpGet]
        public ActionResult Login() {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Member model, int RoleID) {
            Session["Member"] = null;
            //Kullanıcı bilgileri kontrol ediliyor.
            Member user = MemberRepository.db.Members.SingleOrDefault(t => t.Email == model.Email && t.Password == model.Password && t.RoleID == model.RoleID);
            Session["Member"] = user;

            if (user != null && user.RoleID == 1) {
                return RedirectToAction("MemberList", "Member", new { area = "Admin" });
            }
            else if (user != null && user.RoleID == 2) {
                return RedirectToAction("Index", "Home");
            }
            else {
                ViewBag.ErrorGiris = "Şifre, Email veya giriş tercihi hatalı.";
            }
            return View();
        }

        public ActionResult Logout() {
            Session["Member"] = null;
            return RedirectToAction("Index", "Home", new { @cikisMesaj = "Oturum kapatıldı." });
        }

        [HttpGet]
        public ActionResult UserDetail() {
            Member currentUser = (Member)Session["Member"];
            result.ResultT = mr.GetObjByID(currentUser.UserID);
            return View(result.ResultT.ProcessResult);
        }

        [HttpPost]
        public ActionResult UserDetail(Member model, string Password, string FirstName, string LastName, string Address) {

            model.FirstName = FirstName;
            model.LastName = LastName;
            model.Address = Address;
            model.Password = Password;
            result.ResultInt = mr.Update(model);

            if (result.ResultInt.IsSucceeded) {
                //Session["Member"] = MemberRepository.db.Members.SingleOrDefault(t => t.Email == currentUser.Email);
                return RedirectToAction("Index", "Home", new { @UyeBilgileriUpdate = "Üye bilgileri güncellendi." });
            }

            ViewBag.hataMesaj = "Bir hata oluştu.";
            return RedirectToAction("UserDetail");
        }

        public ActionResult DeleteUser() {
            Member currentUser = (Member)Session["Member"];
            result.ResultInt = mr.Delete(currentUser.UserID);
            Session["Member"] = null;
            return RedirectToAction("Index", "Home", new { @deleteMesaj = "Kullanıcı silindi." });
        }

        [HttpGet]
        public ActionResult ShoppingDetail() {
            Member currentUser = (Member)Session["Member"];

            return View();
        }
    }
}