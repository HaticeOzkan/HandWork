using DAL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HandWork.Controllers
{
    public class HomeController : Controller
    {
        HandWorkContext Db = new HandWorkContext();
        public ActionResult Index(int? DisLike,int? Like)
        {
            if (Like != null)
            {
                Product product= Db.Products.Find(DisLike);
                product.LikeCount++;
                Db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (DisLike != null)
            {
                Product product = Db.Products.Find(DisLike);
                product.DisLikeCount++;
                Db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }     
            ViewBag.ProductList = Db.Products.ToList(); 
            return View();
        }

        public ActionResult ProductDetail(int id)
        {
            Product product = Db.Products.Find(id);
            return View(product);
        }

        public ActionResult Buy()
        {
           

            return View();
        }
    }
}