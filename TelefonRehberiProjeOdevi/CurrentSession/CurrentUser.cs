using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TelefonRehberiProjeOdevi.Models;

namespace TelefonRehberiProjeOdevi.CurrentSession
{
    public class CurrentUser
    {
        public static Personel User { get { return Get<Personel>("login");}}
        public static Roller   UserRole { get { return Get<Roller>("rol");}}
        

        
        public static void Set<T>(string key,T obj)
        {
            HttpContext.Current.Session[key] = obj;
        }
        public static T Get<T>(string key)
        {
            if(HttpContext.Current.Session[key] != null)
            {
                return (T)HttpContext.Current.Session[key];
            }

            return default(T);
        }
        public static void Remove<T>(string key)
        {
                 HttpContext.Current.Session.Remove(key);
        }
        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}