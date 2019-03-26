using BLL;
using DAL;
using Entity;
using HandWork.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HandWork.Controllers
{
    public class HomeController : Controller
    {
        UnitOfWork _uw = new UnitOfWork();
        public ActionResult Index()
        {           
                return View(_uw.CategoryRepo.GetAll());         
        }
        public ActionResult Success()
        {
            Member member = User.GetMember();
            ViewBag.BasketID = member.Basket.ID;
            return View();
        }

    }
}