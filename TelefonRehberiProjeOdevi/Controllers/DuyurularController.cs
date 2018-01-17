using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TelefonRehberiProjeOdevi.CurrentSession;
using TelefonRehberiProjeOdevi.Models;

namespace TelefonRehberiProjeOdevi.Controllers
{
    [Auth]
    public class DuyurularController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Duyurular
        public ActionResult Index()
        {
            return View(db.Duyurular.Where(x=>x.Departman.Id==CurrentUser.User.Departman.Id).OrderByDescending(c=>c.Id).ToList());
        }

        // GET: Duyurular/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Duyurular duyurular = db.Duyurular.Find(id);
            if (duyurular == null)
            {
                return HttpNotFound();
            }
            return View(duyurular);
        }

        // GET: Duyurular/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Duyurular/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Baslik,Icerik")] Duyurular duyurular)
        {
            if (ModelState.IsValid)
            {
                duyurular.EklenmeTarihi = DateTime.Now;
                // db.Duyurular.Add(duyurular);
                var ekleyenkisi = CurrentUser.User;
                SqlParameter baslik = new SqlParameter("@baslik", duyurular.Baslik);
                SqlParameter icerik = new SqlParameter("@icerik", duyurular.Icerik);
                SqlParameter eklenmeTarihi = new SqlParameter("@eklenmeTarihi", duyurular.EklenmeTarihi);
            
                SqlParameter departmanId = new SqlParameter("@departmanID", ekleyenkisi.Departman.Id);
                SqlParameter ekleyenId = new SqlParameter("@ekleyenId", ekleyenkisi.Id);

                db.Database.ExecuteSqlCommand("sp_DuyuruEkle @baslik, @icerik, @eklenmeTarihi,@departmanID,@ekleyenId", baslik, icerik,eklenmeTarihi,departmanId,ekleyenId);                    
             
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(duyurular);
        }

        // GET: Duyurular/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
             Duyurular duyurular = db.Duyurular.Find(id);

            if (duyurular == null)
            {
                return HttpNotFound();
            }
            return View(duyurular);
        }

        // POST: Duyurular/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Duyurular duyurular)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(duyurular).State = EntityState.Modified;
                var ekleyenkisi = CurrentUser.User;
                SqlParameter duyuruNo = new SqlParameter("@duyuruId", duyurular.Id);
                SqlParameter baslik = new SqlParameter("@baslik", duyurular.Baslik);
                SqlParameter icerik = new SqlParameter("@icerik", duyurular.Icerik);
                SqlParameter departmanId = new SqlParameter("@departmanID", duyurular.Departman.Id);
                SqlParameter ekleyenId = new SqlParameter("@ekleyenId", duyurular.Id);

                db.Database.ExecuteSqlCommand("sp_DuyuruGuncelle @duyuruId,@baslik, @icerik,@departmanID,@ekleyenId", duyuruNo,baslik, icerik,departmanId,ekleyenId);


                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(duyurular);
        }

        // GET: Duyurular/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Duyurular duyurular = db.Duyurular.Find(id);
            if (duyurular == null)
            {
                return HttpNotFound();
            }
            return View(duyurular);
        }

        // POST: Duyurular/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Duyurular duyurular = db.Duyurular.Find(id);
            // db.Duyurular.Remove(duyurular);
            SqlParameter duyuruNo = new SqlParameter("@duyuruId", duyurular.Id);
            db.Database.ExecuteSqlCommand("sp_DuyuruSil @duyuruId",duyuruNo);

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
