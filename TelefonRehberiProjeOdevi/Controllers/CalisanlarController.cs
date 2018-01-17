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
    [Auth(Roles ="Admin,Müdür,Genel Müdür,Çalışan")]
    public class CalisanlarController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        
        public ActionResult Index()
        {
            if(CurrentUser.User!=null&&CurrentUser.UserRole.RolAdi=="Admin") return View(db.Calisanlar.ToList());
           
            
            return View(db.Calisanlar.Where(x=>x.Personel.Departman.Id==CurrentUser.User.Departman.Id).ToList());
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calisanlar calisanlar = db.Calisanlar.Find(id);
            if (calisanlar == null)
            {
                return HttpNotFound();
            }
            return View(calisanlar);
        }

        [Auth(Roles = "Admin,Genel Müdür,Müdür")]
        public ActionResult Create()
        {
            ViewBag.Roller = new SelectList(db.Roller, "Id", "RolAdi");
            ViewBag.Departmanlar = new SelectList(db.Departmanlar, "Id", "DepartmanAdi");
            return View();
        }

        // POST: Calisanlar/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Calisanlar calisanlar)
        {
            var dbuser = db.GirisBilgileri.FirstOrDefault(x => x.KullaniciAdi == calisanlar.Personel.GirisBilgileri.KullaniciAdi && x.Email == calisanlar.Personel.GirisBilgileri.Email);
            if (dbuser != null && dbuser.Id != calisanlar.Personel.GirisBilgileri.Id)
            {
                if (dbuser.KullaniciAdi == calisanlar.Personel.GirisBilgileri.KullaniciAdi )
                {
                    ModelState.AddModelError("", "Kullanıcı adı zaten kayıtlı");

                }
                if (dbuser.Email == calisanlar.Personel.GirisBilgileri.Email)
                {
                    ModelState.AddModelError("", "E-mail zaten kayıtlı");

                }
                return View(calisanlar);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    calisanlar.Personel.Departman = db.Departmanlar.Find(calisanlar.Personel.Departman.Id);
                    calisanlar.Personel.Rol = db.Roller.FirstOrDefault(x => x.RolAdi == "Çalışan");
                    calisanlar.BaslangicTarihi = DateTime.Now;
                    calisanlar.Personel.DegistirenKisi = CurrentSession.CurrentUser.User.Ad + " " + CurrentSession.CurrentUser.User.Soyad;
                    db.Calisanlar.Add(calisanlar);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }
            ViewBag.Roller = new SelectList(db.Roller, "Id", "RolAdi");
            ViewBag.Departmanlar = new SelectList(db.Departmanlar, "Id", "DepartmanAdi");
            return View(calisanlar);
        }

        [Auth(Roles = "Admin,Genel Müdür,Müdür")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calisanlar calisanlar = db.Calisanlar.Find(id);
            if (calisanlar == null)
            {
                return HttpNotFound();
            }
            ViewBag.Roller = new SelectList(db.Roller, "Id", "RolAdi");
            ViewBag.Departmanlar = new SelectList(db.Departmanlar, "Id", "DepartmanAdi");
            return View(calisanlar);
        }

        // POST: Calisanlar/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Calisanlar calisanlar,string YeniSifre)
        {
            ViewBag.Roller = new SelectList(db.Roller, "Id", "RolAdi");
            ViewBag.Departmanlar = new SelectList(db.Departmanlar, "Id", "DepartmanAdi");
            var dbuser = db.GirisBilgileri.FirstOrDefault(x => x.KullaniciAdi == calisanlar.Personel.GirisBilgileri.KullaniciAdi && x.Email == calisanlar.Personel.GirisBilgileri.Email);
            if (dbuser != null && dbuser.Id != calisanlar.Personel.GirisBilgileri.Id)
            {
                if (dbuser.KullaniciAdi == calisanlar.Personel.GirisBilgileri.KullaniciAdi)
                {
                    ModelState.AddModelError("", "Kullanıcı adı zaten kayıtlı");

                }
                if (dbuser.Email == calisanlar.Personel.GirisBilgileri.Email)
                {
                    ModelState.AddModelError("", "E-mail zaten kayıtlı");

                }
                return View(calisanlar);
            }
            if (ModelState.IsValid)
            {
                if (db.Personel.FirstOrDefault(x => x.GirisBilgileri.Sifre == calisanlar.Personel.GirisBilgileri.Sifre) == null)
                {
                    ModelState.AddModelError("", "Girilen Eski Şifre eşleşmedi.");
                   
                    ViewBag.Departmanlar = new SelectList(db.Departmanlar, "Id", "DepartmanAdi");
                    return View(calisanlar);
                }
                Calisanlar data = db.Calisanlar.Find(calisanlar.Id);

                //Giriş Bilgileri Güncellemesi
                data.Personel.GirisBilgileri.KullaniciAdi = calisanlar.Personel.GirisBilgileri.KullaniciAdi;
                data.Personel.GirisBilgileri.Sifre = calisanlar.Personel.GirisBilgileri.Sifre;
                if (!string.IsNullOrEmpty(YeniSifre))
                {
                    data.Personel.GirisBilgileri.Sifre = YeniSifre;
                }
                data.Personel.GirisBilgileri.Email = calisanlar.Personel.GirisBilgileri.Email;
                //Personel Bilgileri Güncellemesi
                data.Personel.Ad = calisanlar.Personel.Ad;
                data.Personel.Soyad = calisanlar.Personel.Soyad;
                data.Personel.Telefon = calisanlar.Personel.Telefon;
                data.Personel.Departman = db.Departmanlar.Find(calisanlar.Personel.Departman.Id);
                data.Personel.Rol = db.Roller.FirstOrDefault(x => x.RolAdi == "Çalışan");
                data.Personel.DegistirenKisi = CurrentSession.CurrentUser.User.Ad + " " + CurrentSession.CurrentUser.User.Soyad;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(calisanlar);
        }

        [Auth(Roles = "Admin,Genel Müdür,Müdür")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calisanlar calisanlar = db.Calisanlar.Find(id);
            if (calisanlar == null)
            {
                return HttpNotFound();
            }
            return View(calisanlar);
        }

        // POST: Calisanlar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Calisanlar calisanlar = db.Calisanlar.Find(id);
            db.Calisanlar.Remove(calisanlar);
            db.Personel.Remove(db.Personel.Find(calisanlar.Personel.Id));
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
