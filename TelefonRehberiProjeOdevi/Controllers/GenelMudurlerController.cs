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
    [Auth(Roles = "Admin,Genel Müdür")]
    public class GenelMudurlerController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: GenelMudurler
        public ActionResult Index()
        {
            if (CurrentSession.CurrentUser.UserRole.RolAdi == "Admin") return View(db.GenelMudurler.ToList());


            return View(db.GenelMudurler.Where(x => x.Personel.Departman.Id == CurrentUser.User.Departman.Id).ToList());
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GenelMudurler genelMudurler = db.GenelMudurler.Find(id);
            if (genelMudurler == null)
            {
                return HttpNotFound();
            }
            return View(genelMudurler);
        }

       
        public ActionResult Create()
        {
            ViewBag.Roller = new SelectList(db.Roller, "Id", "RolAdi");
            ViewBag.Departmanlar = new SelectList(db.Departmanlar, "Id", "DepartmanAdi");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Roles ="Admin")]
        public ActionResult Create(GenelMudurler genelMudurler)
        {
            ViewBag.Roller = new SelectList(db.Roller, "Id", "RolAdi");
            ViewBag.Departmanlar = new SelectList(db.Departmanlar, "Id", "DepartmanAdi");
            var dbuser = db.GirisBilgileri.FirstOrDefault(x => x.KullaniciAdi == genelMudurler.Personel.GirisBilgileri.KullaniciAdi && x.Email == genelMudurler.Personel.GirisBilgileri.Email);
            if (dbuser != null)
            {
                if (dbuser.KullaniciAdi == genelMudurler.Personel.GirisBilgileri.KullaniciAdi)
                {
                    ModelState.AddModelError("", "Kullanıcı adı zaten kayıtlı");

                }
                if (dbuser.Email == genelMudurler.Personel.GirisBilgileri.Email)
                {
                    ModelState.AddModelError("", "E-mail zaten kayıtlı");

                }
                return View(genelMudurler);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    genelMudurler.Personel.Departman = db.Departmanlar.Find(genelMudurler.Personel.Departman.Id);
                    genelMudurler.Personel.Rol = db.Roller.FirstOrDefault(x => x.RolAdi == "Genel Müdür");
                    genelMudurler.BaslangicTarihi = DateTime.Now;
                    genelMudurler.Personel.DegistirenKisi = CurrentSession.CurrentUser.User.Ad + " " + CurrentSession.CurrentUser.User.Soyad;
                    db.GenelMudurler.Add(genelMudurler);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            return View(genelMudurler);
        }

        [Auth(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GenelMudurler genelMudurler = db.GenelMudurler.Find(id);
            if (genelMudurler == null)
            {
                return HttpNotFound();
            }
            ViewBag.Roller = new SelectList(db.Roller, "Id", "RolAdi");
            ViewBag.Departmanlar = new SelectList(db.Departmanlar, "Id", "DepartmanAdi");
            return View(genelMudurler);
        }

        // POST: GenelMudurler/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GenelMudurler genelMudurler,string YeniSifre)
        {
            ViewBag.Roller = new SelectList(db.Roller, "Id", "RolAdi");
            ViewBag.Departmanlar = new SelectList(db.Departmanlar, "Id", "DepartmanAdi");
            var dbuser = db.GirisBilgileri.FirstOrDefault(x => x.KullaniciAdi == genelMudurler.Personel.GirisBilgileri.KullaniciAdi && x.Email == genelMudurler.Personel.GirisBilgileri.Email);
            if (dbuser != null && dbuser.Id != genelMudurler.Personel.GirisBilgileri.Id)
            {
                if (dbuser.KullaniciAdi == genelMudurler.Personel.GirisBilgileri.KullaniciAdi)
                {
                    ModelState.AddModelError("", "Kullanıcı adı zaten kayıtlı");

                }
                if (dbuser.Email == genelMudurler.Personel.GirisBilgileri.Email)
                {
                    ModelState.AddModelError("", "E-mail zaten kayıtlı");

                }
                return View(genelMudurler);
            }
            if (ModelState.IsValid)
            {
                ViewBag.Roller = new SelectList(db.Roller, "Id", "RolAdi");
                ViewBag.Departmanlar = new SelectList(db.Departmanlar, "Id", "DepartmanAdi");
                if (db.Personel.FirstOrDefault(x => x.GirisBilgileri.Sifre == genelMudurler.Personel.GirisBilgileri.Sifre) == null)
                {
                    ModelState.AddModelError("", "Girilen Eski Şifre eşleşmedi.");
                   
                    return View(genelMudurler);
                }
                GenelMudurler data = db.GenelMudurler.Find(genelMudurler.Id);

                //Giriş Bilgileri Güncellemesi
                data.Personel.GirisBilgileri.KullaniciAdi = genelMudurler.Personel.GirisBilgileri.KullaniciAdi;
                data.Personel.GirisBilgileri.Sifre = genelMudurler.Personel.GirisBilgileri.Sifre;
                if (!string.IsNullOrEmpty(YeniSifre))
                {
                    data.Personel.GirisBilgileri.Sifre = YeniSifre;
                }
                data.Personel.GirisBilgileri.Email = genelMudurler.Personel.GirisBilgileri.Email;
                //Personel Bilgileri Güncellemesi
                data.Personel.Ad = genelMudurler.Personel.Ad;
                data.Personel.Soyad = genelMudurler.Personel.Soyad;
                data.Personel.Telefon = genelMudurler.Personel.Telefon;
                data.Personel.Departman = db.Departmanlar.Find(genelMudurler.Personel.Departman.Id);
                data.Personel.Rol = db.Roller.FirstOrDefault(x => x.RolAdi == "Genel Müdür");
                data.Personel.DegistirenKisi = CurrentSession.CurrentUser.User.Ad + " " + CurrentSession.CurrentUser.User.Soyad;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(genelMudurler);
        }

        [Auth(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GenelMudurler genelMudurler = db.GenelMudurler.Find(id);
            if (genelMudurler == null)
            {
                return HttpNotFound();
            }
            return View(genelMudurler);
        }

        // POST: GenelMudurler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GenelMudurler genelMudurler = db.GenelMudurler.Find(id);
            db.GenelMudurler.Remove(genelMudurler);
            db.Personel.Remove(db.Personel.Find(genelMudurler.Personel.Id));
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
