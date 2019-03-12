using BLL;
using Entity;
using Microsoft.AspNet.Identity;
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
            ViewBag.Categories = _uw.CategoryRepo.GetAll().Select(x => new SelectListItem {
                Text = x.CategoryName,
                Value = (x.ID).ToString()
            });
            return View();

        }
        [HttpPost]
        public ActionResult AddProduct(Product NewProduct,HttpPostedFileBase[] images)
        {
            ViewBag.Categories = _uw.CategoryRepo.GetAll().Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = (x.ID).ToString()
            });
            if (ModelState.IsValid)
            {
                NewProduct.MemberID = User.Identity.GetUserId();           
            if (images != null)
            {
                    int Count = 0;
                  NewProduct.ProductImages = new List<ProductImage>();
                foreach (HttpPostedFileBase item in images)
                {                 
                    ProductImage NewImage = new ProductImage();
                    string Path = Server.MapPath("/Uploads/Products/");//dosya yolu
                    item.SaveAs(Path + Count + ".jpg");//image ismi                 
                    NewImage.ImageURL = "/Uploads/Products/" + Count + ".jpg";                     
                    NewProduct.ProductImages.Add(NewImage);
                        Count++;

                    }
                NewProduct.HasPhoto = true;             
                    _uw.ProductRepo.Add(NewProduct);
                    _uw.Complete();
                }
          
            return View();
            }
            return View();
        }
        public ActionResult DeleteProduct()
        {
            string ID = User.Identity.GetUserId();
          
            return RedirectToAction("MemberProfile", "Member");
        }
    }
}