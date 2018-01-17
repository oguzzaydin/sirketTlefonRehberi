using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TelefonRehberiProjeOdevi.Models
{
    public class Calisanlar:EntityBase
    {
        public DateTime BaslangicTarihi { get; set; }


        public virtual Personel Personel { get; set; }
    }
}