using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TelefonRehberiProjeOdevi.Models
{
    public class Departmanlar:EntityBase
    {
        
        public string DepartmanAdi { get; set; }
        public virtual Personel Yonetici { get; set; }
        public virtual List<Personel> Calisanlar { get; set; }
    }
}