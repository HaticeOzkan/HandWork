using BLL;
using Entity;
using Entity.ViewModel;
using HandWork.Extensions;
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
            if (Name == null)
            {
                TempData["Error"] = "Kayıt Bulunamadı";
                return View();
            }
            ApplicationSignInManager signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            SignInStatus Result = signInManager.PasswordSignIn(Name, LoginModel.Password, true, false);
            switch (Result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");
                case SignInStatus.Failure:
                    {
                        TempData["Error"] = "Hatalı giriş";
                        return View();
                    }                 
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
                TempData["SuccessRegister"] = "Kayıt Gerçekleştirildi";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Error"] = "Şifre En az 8 karakter içermeli rakam ,özel karakter Büyük ve küçük harf içermeli";
                return RedirectToAction("Register", "Member");
            }
            
        }
        [HttpGet]
        public ActionResult AccountEdit()
        {
            Member Member = User.GetMember(_uw);
            return View(Member);
        }
        [HttpPost]
        public ActionResult AccountEdit(Member NewMember, HttpPostedFileBase image)
        {

            Member OldMember = _uw.Db.Users.Find(User.Identity.GetUserId());
            OldMember.UserName = NewMember.UserName;
            OldMember.Email = NewMember.Email;
            OldMember.Address = NewMember.Address;
            OldMember.PhoneNumber = NewMember.PhoneNumber;
            _uw.ProfilPhotoRepo.Delete(OldMember.ProfilPhoto.ID);
            if (image != null)
            {
                OldMember.ProfilPhoto = new ProfilPhoto();
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
        [HttpGet]
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
            bool Result = Manager.CheckPassword(Member, ViewModel.CurrentPassword);//??
            if (ViewModel.NewPassword != ViewModel.ConfirmPassword)
            {
                ViewBag.Result = "Şifreler Uyuşmuyor!";
                return View(); ;
            }
           else if (Result == true)
            {               
                IdentityResult Result2 = Manager.ChangePassword(MemberID, ViewModel.CurrentPassword, ViewModel.NewPassword);
                if (Result2.Succeeded)
                {
                    ViewBag.Result = "Başarılı";
                    return View();
                }
                ViewBag.Result = "Tekrar Deneyin";
                return View();
            }
            else
                ViewBag.Result = "Şifre Yanlış";
            return View();

        }

        public ActionResult MyProfile()
        {
            Member Member = User.GetMember(_uw);
            return View(Member);
        }
        public ActionResult DeleteAccount()
        {
            Member Member = User.GetMember(_uw);
            UserStore<Member> Store = new UserStore<Member>(_uw.Db);
            UserManager<Member> Manager = new UserManager<Member>(Store);
            Manager.Delete(Member);
            return RedirectToAction("Index", "Home");

        }
        public ActionResult MemberProfile(int id)
        {
            Product product = _uw.ProductRepo.GetOne(id);
            Member member = product.Member;
            List<Product> Products = member.Products.ToList();
            ViewBag.ProductList = Products;
            return View(member);
        }      
        public JsonResult MemberComplaint(string id)//şikayet edilecek olan kişinin id si
        {
            //beğeni ve şikayetlere ve dislike kontrol konulmalı kişi birden fazla begenip şikayet edemesin
            Member member = _uw.Db.Users.Find(id);
            member.ComplaintCount = member.ComplaintCount + 1;
            _uw.Db.Entry(member).State = System.Data.Entity.EntityState.Modified;
            _uw.Complete();
            return Json(true);
        }
        public JsonResult SuggestCount(string id)//şikayet edilecek olan kişinin id si
        {
            //beğeni ve şikayetlere ve dislike kontrol konulmalı kişi birden fazla begenip şikayet edemesin
            Member member = _uw.Db.Users.Find(id);
            member.FavorCount = member.FavorCount + 1;
            _uw.Db.Entry(member).State = System.Data.Entity.EntityState.Modified;
            _uw.Complete();
            return Json(true);
        }
        public ActionResult MyConversations()
        {
            Member member = User.GetMember(_uw);      
            return View(member);
        }
        //conversation id geliyor onun mesajları donulecek
        [HttpGet]
        public ActionResult MyMessages(int id)//conversation id si
        {
            Conversation conversation = _uw.Db.Conversations.Find(id);
            List<Message> ListMessage = _uw.Db.Messages.Where(x => x.Conversation == conversation).ToList();
            ViewBag.ConversationId = id;
            return View(ListMessage);
        }
        [HttpPost]
        public ActionResult MyMessages(string NewMesage,string ConID)
        {
            
                return View();
        }
    }
}