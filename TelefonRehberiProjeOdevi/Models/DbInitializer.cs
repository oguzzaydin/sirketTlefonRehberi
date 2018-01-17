using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TelefonRehberiProjeOdevi.Models
{
    public class DbInitializer: CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            Personel data = new Personel()
            {
                Rol = new Roller() { RolAdi = "Admin", RolOncelik = 1 },
                Ad = "Oğuzhan",
                Soyad = "Aydın",
                Departman = new Departmanlar { DepartmanAdi = "Departman 1" }, Telefon = "0000000",
                GirisBilgileri = new GirisBilgileri() { Email = "admin@admin.com", KullaniciAdi = "admin", Sifre = "123", }
            };
            Roller rol = new Roller()
            {
                RolAdi="Genel Müdür",RolOncelik=2,               
            };
            Roller rol2 = new Roller()
            {
                RolAdi = "Müdür",
                RolOncelik = 3,
            };
            Roller rol3 = new Roller()
            {
                RolAdi = "Çalışan",
                RolOncelik = 4,
            };
            Adminler admin = new Adminler()
            {
                BaslangicTarihi = DateTime.Now,
                Personel=data
            };
            CalisanSayisi c1 = new CalisanSayisi()
            {
                Unvan = "Admin",
                Sayisi = 1
            };
            CalisanSayisi c2 = new CalisanSayisi()
            {
                Unvan = "Genel Müdür",
                Sayisi = 0
            };
            CalisanSayisi c3 = new CalisanSayisi()
            {
                Unvan = "Müdür",
                Sayisi = 0
            };
            CalisanSayisi c4 = new CalisanSayisi()
            {
                Unvan = "Çalışan",
                Sayisi = 0
            };
            context.CalisanSayisi.Add(c1);
            context.CalisanSayisi.Add(c2);
            context.CalisanSayisi.Add(c3);
            context.CalisanSayisi.Add(c4);
            context.Roller.Add(rol);
            context.Roller.Add(rol2);
            context.Roller.Add(rol3);
            context.Adminlers.Add(admin);
            context.SaveChanges();
        }
    }
}