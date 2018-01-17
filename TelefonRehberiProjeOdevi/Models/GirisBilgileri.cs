using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TelefonRehberiProjeOdevi.Models
{
    public class GirisBilgileri:EntityBase
    {
     
        public string KullaniciAdi { get; set; }
    
        public string Sifre { get; set; }
        [Display(Name = "E-Mail"),DataType(DataType.EmailAddress,ErrorMessage ="Geçerli bir E-mail adresi giriniz.")]
        public string Email { get; set; }

       
    }
}