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
    [Auth(Roles="Admin")]
    public class RollerController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Roller
        public ActionResult Index()
        {
            return View(db.Roller.ToList());
        }

        // GET: Roller/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Roller roller = db.Roller.Find(id);
            if (roller == null)
            {
                return HttpNotFound();
            }
            return View(roller);
        }

        // GET: Roller/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roller/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RolAdi,RolOncelik")] Roller roller)
        {
            if (ModelState.IsValid)
            {
                db.Roller.Add(roller);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(roller);
        }

        // GET: Roller/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Roller roller = db.Roller.Find(id);
            if (roller == null)
            {
                return HttpNotFound();
            }
            return View(roller);
        }

        // POST: Roller/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RolAdi,RolOncelik")] Roller roller)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roller).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roller);
        }

        // GET: Roller/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Roller roller = db.Roller.Find(id);
            if (roller == null)
            {
                return HttpNotFound();
            }
            return View(roller);
        }

        // POST: Roller/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Roller roller = db.Roller.Find(id);
            db.Roller.Remove(roller);
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
