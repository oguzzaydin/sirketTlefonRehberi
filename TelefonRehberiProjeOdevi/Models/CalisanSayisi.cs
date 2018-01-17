using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TelefonRehberiProjeOdevi.Models
{
    public class CalisanSayisi:EntityBase   
    {
        public string Unvan { get; set; }
        public int  Sayisi { get; set; }
    }
}