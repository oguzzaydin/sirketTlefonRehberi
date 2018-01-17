using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TelefonRehberiProjeOdevi.CurrentSession;
using TelefonRehberiProjeOdevi.Models;

namespace TelefonRehberiProjeOdevi.Controllers
{
    [Auth(Roles = "Admin,Genel Müdür,Müdür")]
    public class MudurlerController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Mudurler
        public ActionResult Index()
        {
            if (CurrentSession.CurrentUser.UserRole.RolAdi == "Admin") return View(db.Mudurler.ToList());

            return View(db.Mudurler.Where(x => x.Personel.Departman.Id == CurrentUser.User.Departman.Id).ToList());
        }

        // GET: Mudurler/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mudurler mudurler = db.Mudurler.Find(id);
            if (mudurler == null)
            {
                return HttpNotFound();
            }
            return View(mudurler);
        }

        [Auth(Roles ="Admin,Genel Müdür")]
        public ActionResult Create()
        {
            ViewBag.Roller = new SelectList(db.Roller, "Id", "RolAdi");
            ViewBag.Departmanlar = new SelectList(db.Departmanlar, "Id", "DepartmanAdi");
            return View();
        }

        // POST: Mudurler/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Mudurler mudurler)
        {
            ViewBag.Roller = new SelectList(db.Roller, "Id", "RolAdi");
            ViewBag.Departmanlar = new SelectList(db.Departmanlar, "Id", "DepartmanAdi");
            var dbuser = db.GirisBilgileri.FirstOrDefault(x => x.KullaniciAdi == mudurler.Personel.GirisBilgileri.KullaniciAdi && x.Email == mudurler.Personel.GirisBilgileri.Email);
            if (dbuser != null)
            {
                if (dbuser.KullaniciAdi == mudurler.Personel.GirisBilgileri.KullaniciAdi)
                {
                    ModelState.AddModelError("", "Kullanıcı adı zaten kayıtlı");

                }
                if (dbuser.Email == mudurler.Personel.GirisBilgileri.Email)
                {
                    ModelState.AddModelError("", "E-mail zaten kayıtlı");

                }
                return View(mudurler);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    mudurler.Personel.Departman = db.Departmanlar.Find(mudurler.Personel.Departman.Id);
                    mudurler.Personel.Rol = db.Roller.FirstOrDefault(x=>x.RolAdi=="Müdür");
                    mudurler.BaslangicTarihi = DateTime.Now;
                    mudurler.Personel.DegistirenKisi = CurrentSession.CurrentUser.User.Ad + " " + CurrentSession.CurrentUser.User.Soyad;
                    db.Mudurler.Add(mudurler);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            return View(mudurler);
        }

        [Auth(Roles = "Admin,Genel Müdür")]
        public ActionResult Edit(int? id)
        {
            ViewBag.Roller = new SelectList(db.Roller, "Id", "RolAdi");
            ViewBag.Departmanlar = new SelectList(db.Departmanlar, "Id", "DepartmanAdi");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mudurler mudurler = db.Mudurler.Find(id);
            if (mudurler == null)
            {
                return HttpNotFound();
            }
            return View(mudurler);
        }

        // POST: Mudurler/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Mudurler mudurler,string YeniSifre)
        {
            ViewBag.Roller = new SelectList(db.Roller, "Id", "RolAdi");
            ViewBag.Departmanlar = new SelectList(db.Departmanlar, "Id", "DepartmanAdi");
            var dbuser = db.GirisBilgileri.FirstOrDefault(x => x.KullaniciAdi == mudurler.Personel.GirisBilgileri.KullaniciAdi && x.Email == mudurler.Personel.GirisBilgileri.Email);
            if (dbuser != null && dbuser.Id != mudurler.Personel.GirisBilgileri.Id)
            {
                if (dbuser.KullaniciAdi == mudurler.Personel.GirisBilgileri.KullaniciAdi)
                {
                    ModelState.AddModelError("", "Kullanıcı adı zaten kayıtlı");

                }
                if (dbuser.Email == mudurler.Personel.GirisBilgileri.Email)
                {
                    ModelState.AddModelError("", "E-mail zaten kayıtlı");

                }
                return View(mudurler);
            }
            if (ModelState.IsValid)
            {
                if (db.Personel.FirstOrDefault(x => x.GirisBilgileri.Sifre == mudurler.Personel.GirisBilgileri.Sifre) == null)
                {
                    ModelState.AddModelError("", "Girilen Eski Şifre eşleşmedi.");
                    ViewBag.Roller = new SelectList(db.Roller, "Id", "RolAdi");
                    ViewBag.Departmanlar = new SelectList(db.Departmanlar, "Id", "DepartmanAdi");
                    return View(mudurler);
                }
                Mudurler data = db.Mudurler.Find(mudurler.Id);

                //Giriş Bilgileri Güncellemesi
                data.Personel.GirisBilgileri.KullaniciAdi = mudurler.Personel.GirisBilgileri.KullaniciAdi;
                data.Personel.GirisBilgileri.Sifre = mudurler.Personel.GirisBilgileri.Sifre;
                if (!string.IsNullOrEmpty(YeniSifre))
                {
                    data.Personel.GirisBilgileri.Sifre = YeniSifre;
                }
                data.Personel.GirisBilgileri.Email = mudurler.Personel.GirisBilgileri.Email;
                //Personel Bilgileri Güncellemesi
                data.Personel.Ad = mudurler.Personel.Ad;
                data.Personel.Soyad = mudurler.Personel.Soyad;
                data.Personel.Telefon = mudurler.Personel.Telefon;
                data.Personel.Departman = db.Departmanlar.Find(mudurler.Personel.Departman.Id);
                data.Personel.Rol = db.Roller.FirstOrDefault(x => x.RolAdi == "Müdür");
                data.Personel.DegistirenKisi = CurrentSession.CurrentUser.User.Ad + " " + CurrentSession.CurrentUser.User.Soyad;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mudurler);
        }

        [Auth(Roles = "Admin,Genel Müdür")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mudurler mudurler = db.Mudurler.Find(id);
            if (mudurler == null)
            {
                return HttpNotFound();
            }
            return View(mudurler);
        }

        // POST: Mudurler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mudurler mudurler = db.Mudurler.Find(id);
            db.Mudurler.Remove(mudurler);
            db.Personel.Remove(db.Personel.Find(mudurler.Personel.Id));
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
