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
        public long TC { get; set; }
      
        public Gender Gender { get; set; }
       public string Address { get; set; }
        public virtual List<Product> Products { get; set; }
        public virtual Basket Basket { get; set; }
        public virtual List<Order> Orders { get; set; }
        public string Text { get; set;}
        public virtual ProfilPhoto ProfilPhoto { get; set; }
        public int? ComplaintCount { get; set; }
        public int? FavorCount { get; set; }
        public bool HasPhoto { get; set; }
        public virtual List<Message> Messages { get; set; }
        public virtual List<Notification> Notifications { get; set; }
        public Member()
        {
            HasPhoto = false;
            ComplaintCount = 0;
            FavorCount=0;
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Member> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            Claim c1 = new Claim("TC", this.TC.ToString());
            Claim c2 = new Claim("NameSurname", this.UserName);
           // Claim c3 = new Claim("Address", this.Address);
           
            userIdentity.AddClaim(c1);
            userIdentity.AddClaim(c2);
           // userIdentity.AddClaim(c3);
            return userIdentity;
        }

    }
}
