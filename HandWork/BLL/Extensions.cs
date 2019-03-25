using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public static class Extensions
    {
        public static string GetTC(this IIdentity Id)
        {
            var allId = (ClaimsIdentity)Id;
            string TC = allId.FindFirst("TC").Value;//claimi TC olanın valuesini getir claimsin içindeki yazıyla aynı olucak
            return TC;
        }
        public static string GetNameSurname(this IIdentity id)
        {
            var allId = (ClaimsIdentity)id;
            string NameSurname = allId.FindFirst("NameSurname").Value;
            return NameSurname;
        }
       public static string GetAdress(this IIdentity id)
        {
            var allId = (ClaimsIdentity)id;
            string Address = allId.FindFirst("Address").Value;
            return Address;
        }
    }
}
