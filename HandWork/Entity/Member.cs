using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public enum Gender
    {
        Female,
        Male
    }
   public class Member:IdentityUser
    {//email password telno geliyor identity userdan
        public string NameSurname { get; set; }
        public Gender Gender { get; set; }
        public string Adress { get; set; }
        public virtual List<Product> Products { get; set; }
        public virtual Basket Basket { get; set; }
        public virtual List<Order> Orders { get; set; }
        public int Age { get; set; }
        public string Text { get; set; }
        public virtual ProfilPhoto ProfilPhoto { get; set; }
        public int ComplaintCount { get; set; }
        public int FavorCount { get; set; }
        public virtual NotificationBox NotificationBox { get; set; }
        public virtual List<NotificationItem> NotificationItemsFromMe { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Member> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

    }
}
