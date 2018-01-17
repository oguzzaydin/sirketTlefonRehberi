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
    public class DepartmanlarController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Departmanlar
        public ActionResult Index()
        {
            return View(db.Departmanlar.ToList());
        }

        // GET: Departmanlar/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departmanlar departmanlar = db.Departmanlar.Find(id);
            if (departmanlar == null)
            {
                return HttpNotFound();
            }
            return View(departmanlar);
        }

       
        public ActionResult Create()
        {
            var kisiler = db.Personel.ToList().Select(s => new { Id = s.Id, Ekleyenkisi = s.Ad + " " + s.Soyad + $"({s.Rol.RolAdi})" });


            ViewBag.Kisiler = new SelectList(kisiler,"Id","Ekleyenkisi");
            return View();
        }

        // POST: Departmanlar/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Departmanlar departmanlar)
        {

            ModelState.Remove("Yonetici.Ad");
            ModelState.Remove("Yonetici.Soyad");
            ModelState.Remove("Yonetici.Telefon");
            var kisiler = db.Personel.ToList().Select(s => new { Id = s.Id, Ekleyenkisi = s.Ad + " " + s.Soyad + $"({s.Rol.RolAdi})"});
            departmanlar.Yonetici = db.Personel.Find(departmanlar.Yonetici.Id);
            ViewBag.Kisiler = new SelectList(kisiler, "Id", "Ekleyenkisi");
            if (ModelState.IsValid)
            {
                db.Departmanlar.Add(departmanlar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(departmanlar);
        }

        // GET: Departmanlar/Edit/5
        public ActionResult Edit(int? id)
        {
            var kisiler = db.Personel.ToList().Select(s => new { Id = s.Id, Ekleyenkisi = s.Ad + " " + s.Soyad + $"({s.Rol.RolAdi})" });
           

           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departmanlar departmanlar = db.Departmanlar.Find(id);
            if (departmanlar == null)
            {
                return HttpNotFound();
            }
            if(departmanlar.Yonetici!=null)
            { 
                ViewBag.Kisiler = new SelectList(kisiler, "Id", "Ekleyenkisi",departmanlar.Yonetici.Id);
            }
            else { ViewBag.Kisiler = new SelectList(kisiler, "Id", "Ekleyenkisi"); }
            return View(departmanlar);
        }

        // POST: Departmanlar/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Departmanlar departmanlar)
        {
            ModelState.Remove("Yonetici.Ad");
            ModelState.Remove("Yonetici.Soyad");
            ModelState.Remove("Yonetici.Telefon");
            var kisiler = db.Personel.ToList().Select(s => new { Id = s.Id, Ekleyenkisi = s.Ad + " " + s.Soyad + $"({s.Rol.RolAdi})" });
            
            ViewBag.Kisiler = new SelectList(kisiler, "Id", "Ekleyenkisi", departmanlar.Yonetici.Id);
            if (ModelState.IsValid)
            {
                Departmanlar departman = db.Departmanlar.Find(departmanlar.Id);
                departman.DepartmanAdi = departmanlar.DepartmanAdi;
                departman.Yonetici = db.Personel.Find(departmanlar.Yonetici.Id);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(departmanlar);
        }

        // GET: Departmanlar/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departmanlar departmanlar = db.Departmanlar.Find(id);
            if (departmanlar == null)
            {
                return HttpNotFound();
            }
            return View(departmanlar);
        }

        // POST: Departmanlar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Departmanlar departmanlar = db.Departmanlar.Find(id);
            db.Departmanlar.Remove(departmanlar);
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
