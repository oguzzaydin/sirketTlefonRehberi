using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TelefonRehberiProjeOdevi.Models
{
    public class Roller : EntityBase
    {
      
        public string RolAdi { get; set; }
      
        public int RolOncelik { get; set; }
        public virtual List<Personel> Kisiler { get; set; }
    }
}