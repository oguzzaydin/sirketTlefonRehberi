using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TelefonRehberiProjeOdevi.CurrentSession;
using TelefonRehberiProjeOdevi.Models;

namespace TelefonRehberiProjeOdevi.Controllers
{
    public class HomeController : Controller
    {
        DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
           if(CurrentUser.User!=null)
            {
                if (CurrentUser.User.Rol.RolAdi == "Admin") return Redirect("~/Adminler");
                else if (CurrentUser.User.Rol.RolAdi == "Müdür") return Redirect("~/Mudurler");
                else if (CurrentUser.User.Rol.RolAdi == "Genel Müdür") return Redirect("~/GenelMudurler");
                else if (CurrentUser.User.Rol.RolAdi == "Çalışan") return Redirect("~/Calisanlar");
            }
            return View();
        }
        public ActionResult YetkiYok()
        {

            return View();
        }
        public ActionResult Cikis()
        {
            CurrentSession.CurrentUser.Clear();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Index(LoginViewModel login)
        {
            if(ModelState.IsValid)
            {
                var user = db.Personel.Include(x => x.GirisBilgileri).Where(x => x.GirisBilgileri.KullaniciAdi == login.username && x.GirisBilgileri.Sifre ==login.password).FirstOrDefault();

                if (user != null)
                {
                    CurrentUser.Set<Personel>("login",user);
                    CurrentUser.Set<Roller>("rol", user.Rol);
                    if (user.Rol.RolAdi == "Admin") return Redirect("~/Adminler");
                    else if (user.Rol.RolAdi == "Müdür") return Redirect("~/Mudurler");
                    else if (user.Rol.RolAdi == "Genel Müdür") return Redirect("~/GenelMudurler");
                    else if (user.Rol.RolAdi == "Çalışan") return Redirect("~/Calisanlar");
                    ModelState.AddModelError("", "Kullanıcı Adı veya Şifre hatalı!");
                }
            }
            login.password="";
            return View(login);
        }
    }
}