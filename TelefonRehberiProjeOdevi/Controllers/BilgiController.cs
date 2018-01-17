using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TelefonRehberiProjeOdevi.Models;

namespace TelefonRehberiProjeOdevi.Controllers
{
    public class BilgiController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        public ActionResult Index()
        {
            return View(db.CalisanSayisi.ToList());
        }
    }
}