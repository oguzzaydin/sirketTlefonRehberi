using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TelefonRehberiProjeOdevi.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Departmanlar> Departmanlar { get; set; }
        public DbSet<Duyurular> Duyurular { get; set; }
        public DbSet<GenelMudurler> GenelMudurler { get; set; }
        public DbSet<GirisBilgileri> GirisBilgileri { get; set; }         
        public DbSet<Mudurler> Mudurler { get; set; }
        public DbSet<Personel> Personel { get; set; }
        public DbSet<Roller> Roller { get; set; }
        public DbSet<Calisanlar> Calisanlar { get; set; }
        public DbSet<CalisanSayisi> CalisanSayisi { get; set; }
        public DatabaseContext()
        {
            Database.SetInitializer(new DbInitializer());
        }

        public System.Data.Entity.DbSet<TelefonRehberiProjeOdevi.Models.Adminler> Adminlers { get; set; }
    }
}