using BLL;
using Entity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace HandWork.Extensions
{
    public static class MemberExtensions
    {
        public static Member GetMember(this IPrincipal Ip,UnitOfWork _uw)//ip burda user i temsil ediyor user.deyince çıkacak
        {          //hangi tip oldugunu userin üstüne geli öğrendik
           
            string MemberID = Ip.Identity.GetUserId();
            Member Member = _uw.Db.Users.Find(MemberID);
            return Member;
        }
    }
}