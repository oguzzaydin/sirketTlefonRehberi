using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TelefonRehberiProjeOdevi.Models
{
    public class Duyurular:EntityBase
    {
      
        public string Baslik  { get; set; }
  
        public string Icerik { get; set; }
        public DateTime EklenmeTarihi { get; set; }

        public virtual Departmanlar Departman { get; set; }
        public virtual Personel EkleyenKisi { get; set; }
    }
}