using BLL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HandWork.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        UnitOfWork _uw = new UnitOfWork();

        public ActionResult Index(int id)
        {
          
            return View();
        }
        [HttpGet]
        public ActionResult AddProduct()
        {
            ViewBag.Category = _uw.CategoryRepo.GetAll();
            return View();

        }
        [HttpPost]
        public ActionResult AddProduct(Product NewProduct)
        {
            ViewBag.Category = _uw.CategoryRepo.GetAll();
            return View();

        }
    }
}