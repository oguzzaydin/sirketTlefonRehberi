using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TelefonRehberiProjeOdevi.Models
{
    public class Mudurler:EntityBase
    {
        public DateTime BaslangicTarihi { get; set; }
      
        

        public virtual Personel Personel { get; set; }
    }
}