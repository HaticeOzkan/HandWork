using BLL;
using Entity;
using HandWork.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HandWork.Controllers
{
    public class MemberController : Controller
    {
        // GET: Member

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel LoginModel)
        {
            ApplicationSignInManager signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            SignInStatus Result = signInManager.PasswordSignIn(LoginModel.Email, LoginModel.Password, true, false);
            switch (Result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");
                case SignInStatus.Failure:
                    return RedirectToAction("Index");

            }
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {

            return View();
        }


        // [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Register(Member NewMember, string Password, HttpPostedFileBase image)
        {
            UserStore<Member> Store = new UserStore<Member>(UnitOfWork.Create());
            UserManager<Member> Manager = new UserManager<Member>(Store);
            var Result = Manager.Create(NewMember, Password);
            if (Result.Succeeded)
            {
                if (image != null)
                {
                    string Path = Server.MapPath("/Uploads/Members/");//dosya yolu
                    image.SaveAs(Path + NewMember.Id + ".jpg");//image ismi
                    NewMember.HasPhoto = true;
                    Manager.Update(NewMember);
                }

            }
            else
              //  ViewBag.Errors = Result.Errors;
              Session["hata"]= Result.Errors;
            return RedirectToAction("Index");
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        public ActionResult Profile()
        {
            return View();
        }
    }
}