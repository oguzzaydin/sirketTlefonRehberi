using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TelefonRehberiProjeOdevi.Models
{
    public class Personel : EntityBase
    {
       
        public string Ad { get; set; }
        
        public string Soyad { get; set; }

        public string Telefon { get; set; }
        public string DegistirenKisi { get; set; }


        public virtual GirisBilgileri GirisBilgileri { get; set; }
        public virtual Departmanlar Departman { get; set; }
      
        public virtual Roller Rol { get; set; }
  
        public virtual List<Duyurular> EkledigiDuyurular { get; set; }
        
       


    }
}