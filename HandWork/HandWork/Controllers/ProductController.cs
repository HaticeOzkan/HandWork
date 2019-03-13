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
            ViewBag.Categories = _uw.CategoryRepo.GetAll().Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = (x.ID).ToString()
            });
            return View();

        }
        [HttpPost]
        public ActionResult AddProduct(Product NewProduct, HttpPostedFileBase[] images)
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

                return RedirectToAction("MemberProfile", "Member");
            }
            return RedirectToAction("MemberProfile", "Member");
        }
        public ActionResult DeleteProduct(int ProductID)
        {

            _uw.ProductRepo.Delete(ProductID);

            return RedirectToAction("MemberProfile", "Member");
        }
        [HttpGet]
        public ActionResult EditProduct(int ProductID)
        {
            ViewBag.Categories = _uw.CategoryRepo.GetAll().Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = (x.ID).ToString()
            });
            Product Product = _uw.ProductRepo.GetOne(ProductID);
            ViewBag.ImageList = _uw.Db.ProductImages.Where(x => x.Product.ID == ProductID).ToList();
            ViewBag.Product = Product;


            return View(Product);
        }
        [HttpPost]
        public ActionResult EditProduct(Product NewProduct, HttpPostedFileBase[] images)
        {//burası bitmedi resimler gelmiyor
            ViewBag.Categories = _uw.CategoryRepo.GetAll().Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = (x.ID).ToString()
            });
            Product OldProduct = _uw.ProductRepo.GetOne(NewProduct.ID);
            _uw.Db.Entry(OldProduct).CurrentValues.SetValues(NewProduct);
            if (images != null)
            {
                int Count = _uw.Db.ProductImages.OrderByDescending(x => x.ID).Select(x => x.ID).FirstOrDefault();
                foreach (HttpPostedFileBase item in images)
                {
                    ProductImage NewImage = new ProductImage();
                    string Path = Server.MapPath("/Uploads/Products/");//dosya yolu
                    item.SaveAs(Path + (Count+1) + ".jpg");//image ismi                 
                    NewImage.ImageURL = "/Uploads/Products/" + (Count + 1) + ".jpg";
                    OldProduct.ProductImages.Add(NewImage);
                    Count++;
                }
            }
            _uw.ProductRepo.Edit(OldProduct);
            _uw.Complete();
            return View();
        }
        [HttpGet]
        public ActionResult ProductDetail(int ProductID)
        {
            string MemberID = User.Identity.GetUserId();
            Member Member = _uw.Db.Users.Find(MemberID);
            return View(Member);
        }
        public JsonResult DeletePic(int id)
        {
            try
            {
                int productImageID = _uw.Db.ProductImages.Where(x => x.ID == id).Select(x => x.ID).FirstOrDefault();
                _uw.ImageRepo.Delete(productImageID);
                _uw.Complete();
                return Json(true);

            }catch(Exception ex)
            {
                return Json(false);
            }

        }

    }
}
