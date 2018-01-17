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
    [Auth(Roles = "Admin")]
    public class AdminlerController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        public ActionResult Index()
        {
            return View(db.Adminlers.ToList());
        }

        // GET: Adminler/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adminler adminler = db.Adminlers.Find(id);
            if (adminler == null)
            {
                return HttpNotFound();
            }
            return View(adminler);
        }

        // GET: Adminler/Create
        public ActionResult Create()
        {
            ViewBag.Roller = new SelectList(db.Roller, "Id", "RolAdi");
            ViewBag.Departmanlar = new SelectList(db.Departmanlar, "Id", "DepartmanAdi");

            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Adminler adminler)
        {
            ViewBag.Roller = new SelectList(db.Roller, "Id", "RolAdi");
            ViewBag.Departmanlar = new SelectList(db.Departmanlar, "Id", "DepartmanAdi");
            var dbuser = db.GirisBilgileri.FirstOrDefault(x => x.KullaniciAdi == adminler.Personel.GirisBilgileri.KullaniciAdi && x.Email == adminler.Personel.GirisBilgileri.Email);
            if (dbuser != null)
            {
                if (dbuser.KullaniciAdi == adminler.Personel.GirisBilgileri.KullaniciAdi)
                {
                    ModelState.AddModelError("", "Kullanıcı adı zaten kayıtlı");

                }
                if (dbuser.Email == adminler.Personel.GirisBilgileri.Email)
                {
                    ModelState.AddModelError("", "E-mail zaten kayıtlı");

                }
                return View(adminler);
            }
            if (ModelState.IsValid)
            {
                try
                { 
                adminler.Personel.Departman = db.Departmanlar.Find(adminler.Personel.Departman.Id);
                    adminler.Personel.Rol = db.Roller.FirstOrDefault(x => x.RolAdi == "Admin");
                    adminler.BaslangicTarihi = DateTime.Now;
                db.Adminlers.Add(adminler);
                db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }
            
            return View(adminler);
        }

        // GET: Adminler/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adminler adminler = db.Adminlers.Find(id);
            if (adminler == null)
            {
                return HttpNotFound();
            }
            ViewBag.Roller = new SelectList(db.Roller, "Id", "RolAdi");
            ViewBag.Departmanlar = new SelectList(db.Departmanlar, "Id", "DepartmanAdi");
            return View(adminler);
        }

        // POST: Adminler/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Adminler adminler,string YeniSifre)
        {
            ViewBag.Roller = new SelectList(db.Roller, "Id", "RolAdi");
            ViewBag.Departmanlar = new SelectList(db.Departmanlar, "Id", "DepartmanAdi");
            var dbuser = db.GirisBilgileri.FirstOrDefault(x => x.KullaniciAdi == adminler.Personel.GirisBilgileri.KullaniciAdi && x.Email == adminler.Personel.GirisBilgileri.Email);
            if (dbuser != null && dbuser.Id != adminler.Personel.GirisBilgileri.Id)
            {
                if (dbuser.KullaniciAdi == adminler.Personel.GirisBilgileri.KullaniciAdi)
                {
                    ModelState.AddModelError("", "Kullanıcı adı zaten kayıtlı");

                }
                if (dbuser.Email == adminler.Personel.GirisBilgileri.Email)
                {
                    ModelState.AddModelError("", "E-mail zaten kayıtlı");

                }
                return View(adminler);
            }
            if (ModelState.IsValid)
            {
              
                if(db.Personel.FirstOrDefault(x=>x.GirisBilgileri.Sifre==adminler.Personel.GirisBilgileri.Sifre)==null)
                {
                    ModelState.AddModelError("", "Girilen Eski Şifre eşleşmedi.");
                   
                    return View(adminler);
                }
                Adminler data = db.Adminlers.Find(adminler.Id);


                        
                //Giriş Bilgileri Güncellemesi
                data.Personel.GirisBilgileri.KullaniciAdi = adminler.Personel.GirisBilgileri.KullaniciAdi;
                data.Personel.GirisBilgileri.Sifre = adminler.Personel.GirisBilgileri.Sifre;
                if(!string.IsNullOrEmpty(YeniSifre))
                {
                    data.Personel.GirisBilgileri.Sifre = YeniSifre;
                }
                data.Personel.GirisBilgileri.Email = adminler.Personel.GirisBilgileri.Email;
                //Personel Bilgileri Güncellemesi
                data.Personel.Ad = adminler.Personel.Ad;
                data.Personel.Soyad = adminler.Personel.Soyad;
                data.Personel.Telefon = adminler.Personel.Telefon;
                data.Personel.Departman = db.Departmanlar.Find(adminler.Personel.Departman.Id);
                data.Personel.Rol = db.Roller.FirstOrDefault(x=>x.RolAdi == "Admin");
                CurrentUser.Set<Personel>("login", data.Personel);
                CurrentUser.Set<Roller>("rol", data.Personel.Rol);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            return View(adminler);
        }

        // GET: Adminler/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adminler adminler = db.Adminlers.Find(id);
            if (adminler == null)
            {
                return HttpNotFound();
            }
            return View(adminler);
        }

        // POST: Adminler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Adminler adminler = db.Adminlers.Find(id);
            db.Adminlers.Remove(adminler);
            db.Personel.Remove(db.Personel.Find(adminler.Personel.Id));
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
