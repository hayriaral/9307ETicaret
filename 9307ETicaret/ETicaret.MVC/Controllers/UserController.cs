﻿using ETicaret.Entity;
using ETicaret.MVC.Models.ResultModel;
using ETicaret.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETicaret.MVC.Controllers
{
    public class UserController : Controller
    {
        MemberRepository mr = new MemberRepository();
        UserInstanceResult<Member> result = new UserInstanceResult<Member>();
        // GET: User
        [HttpGet]
        public ActionResult SignUp(string mesajUye, string mesajPassword)
        {
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
                        return RedirectToAction("Index","Home", new { @yeniUyeMesaj = "Üye olma işlemi başarılı!" });
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
    }
}