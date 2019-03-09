﻿using BLL;
using Entity;
using Entity.ViewModel;
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
        UnitOfWork _uw = new UnitOfWork();

        // GET: Member     
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Entity.ViewModel.LoginViewModel LoginModel)
        {
            string Name = _uw.Db.Users.Where(x => x.Email == LoginModel.Email).Select(x => x.UserName).FirstOrDefault();
            ApplicationSignInManager signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            SignInStatus Result = signInManager.PasswordSignIn(Name, LoginModel.Password, true, false);
            switch (Result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");
                case SignInStatus.Failure:
                    return RedirectToAction("Index");

            }
            return View();
        }
        [ValidateAntiForgeryToken]
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
                TempData["Error"] = Result.Errors;

            return RedirectToAction("Index");
        }
        public ActionResult Account()
        {
            var MemberID = User.Identity.GetUserId();//giriş yapmış olan kullanıcının id sini getirir
            Member member = _uw.Db.Users.Find(MemberID);
            //MemberViewModel memberViewModel = new MemberViewModel();
            //memberViewModel.Adress = member.Adress;
            //memberViewModel.ImageURL = member.ProfilPhoto.ImageURL;
            //memberViewModel.NameSurname = member.UserName;
            //memberViewModel.TelNo = member.PhoneNumber;
            //memberViewModel.Text = member.Text;
            //memberViewModel.HasPhoto = member.HasPhoto;
            return View(member);
       
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}