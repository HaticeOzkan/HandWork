using BLL;
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
        [HttpGet]
        public ActionResult Login()
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
        [HttpGet]
        public ActionResult Register()
        {
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
                    NewMember.ProfilPhoto = new ProfilPhoto();
                    NewMember.ProfilPhoto.ImageURL = "/Uploads/Members/" + NewMember.Id + ".jpg";
                    NewMember.HasPhoto = true;
                    Manager.Update(NewMember);
                }

            }
            else
                TempData["Error"] = Result.Errors;

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult AccountEdit()
        {
            var MemberID = User.Identity.GetUserId();//giriş yapmış olan kullanıcının id sini getirir
            Member member = _uw.Db.Users.Find(MemberID);
            return View(member);
        }
        [HttpPost]
        public ActionResult AccountEdit(Member NewMember, HttpPostedFileBase image)
        {

            Member OldMember = _uw.Db.Users.Find(User.Identity.GetUserId());
            OldMember.UserName = NewMember.UserName;
            OldMember.Email = NewMember.Email;
            OldMember.Adress = NewMember.Adress;
            OldMember.PhoneNumber = NewMember.PhoneNumber;
            _uw.ProfilPhotoRepo.Delete(OldMember.ProfilPhoto.ID);
            OldMember.Gender = NewMember.Gender;
            if (image != null)
            {
                string Path = Server.MapPath("/Uploads/Members/");//dosya yolu
                image.SaveAs(Path + OldMember.Id + ".jpg");//image ismi                 
                OldMember.ProfilPhoto.ImageURL = "/Uploads/Members/" + OldMember.Id + ".jpg";
                OldMember.HasPhoto = true;

            }
            _uw.Db.Entry(OldMember).State = System.Data.Entity.EntityState.Modified;
            _uw.Complete();
            return RedirectToAction("Index", "Home");

        }
        public ActionResult MyProducts()
        {
            var MemberID = User.Identity.GetUserId();
            List<Product> List = _uw.Db.Products.Where(x => x.MemberID == MemberID).ToList();
            return View(List);
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
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasViewModel ViewModel)
        {
            UserStore<Member> Store = new UserStore<Member>(_uw.Db);
            UserManager<Member> Manager = new UserManager<Member>(Store);
            string MemberID = User.Identity.GetUserId();
            Member Member = _uw.Db.Users.Find(MemberID);
            bool Result = Manager.CheckPassword(Member, ViewModel.CurrentPassword);
            if (Result == true)
            {
                IdentityResult Result2 = Manager.ChangePassword(MemberID, ViewModel.CurrentPassword, ViewModel.NewPassword);
                if (Result2.Succeeded)
                {
                    ViewBag.Result = "Başarılı";
                    return RedirectToAction("MyProfile", "Member");
                }
                else
                {
                    ViewBag.Result = "İki şifreyi aynı girmediniz.";
                    return View();
                }

            }
            else
                ViewBag.Result = "Şifre Yanlış";
            return View();

        }

        public ActionResult MyProfile()
        {
            string MemberID = User.Identity.GetUserId();
            Member Member = _uw.Db.Users.Find(MemberID);
            return View(Member);
        }
    }
}