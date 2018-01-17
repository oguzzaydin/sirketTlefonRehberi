using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TelefonRehberiProjeOdevi.CurrentSession
{
    public class Auth : AuthorizeAttribute
    {
        public int yetkisiniri { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            if(CurrentUser.UserRole==null|| CurrentUser.User==null)
            {
                httpContext.Response.Redirect("~/Home");
                return false;
            }
            if (!Roles.Contains(CurrentUser.UserRole.RolAdi)&&Roles!="")
             {
                httpContext.Response.Redirect("~/Home/YetkiYok");
                return false;
            }
           
            return true;
            
        }
    }
}