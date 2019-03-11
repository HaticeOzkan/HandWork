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
            ViewBag.Categories = _uw.CategoryRepo.GetAll().Select(x => new SelectListItem {
                Text = x.CategoryName,
                Value = (x.ID).ToString()
            });
            return View();

        }
        [HttpPost]
        public ActionResult AddProduct(Product NewProduct,IEnumerable<HttpPostedFileBase> images)
        {
            ViewBag.Categories = _uw.CategoryRepo.GetAll().Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = (x.ID).ToString()
            });
            _uw.ProductRepo.Add(NewProduct);
            if (images != null)
            {
                foreach (HttpPostedFileBase item in images)
                {
                    ProductImage NewImage = new ProductImage();
                    string Path = Server.MapPath("/Uploads/Products/");//dosya yolu
                    item.SaveAs(Path + NewProduct.ID + ".jpg");//image ismi
                    NewProduct.ProductImages = new List<ProductImage>();
                    NewImage.ImageURL = "/Uploads/Products/" + NewProduct.ID + ".jpg";
                    NewProduct.ProductImages.Add(NewImage);
                    NewProduct.HasPhoto = true;
                }
                _uw.ProductRepo.Edit(NewProduct);

            }
          
            return View();

        }
    }
}