﻿using BLL;
using Entity;
using HandWork.Extensions;
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
        public ActionResult GetProducts(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                Member Member = User.GetMember(_uw);
                ViewBag.Member = Member;
                return View(_uw.Db.Products.Where(x => x.CategoryID == id && x.StockCount != 0 && x.Member.Id != Member.Id).ToList());
            }
            else
            {
                return View(_uw.Db.Products.Where(x => x.CategoryID == id && x.StockCount != 0).ToList());
            }
          
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
            {    string MemberID= User.Identity.GetUserId();
                NewProduct.MemberID = MemberID;
                _uw.ProductRepo.Add(NewProduct);
                _uw.Complete();
                int ProductID = _uw.Db.Products.Where(x => x.MemberID == MemberID).OrderByDescending(x=>x.ID).Select(x => x.ID).FirstOrDefault();
                if (images != null)
                {
                    //System.IO.Directory.CreateDirectory("C:/Users/funda/source/repos/HandWork/HandWork/HandWork/Uploads/Products/" +ProductID);
                    System.IO.Directory.CreateDirectory("C:/Users/Section1/source/repos/HandWork/HandWork/HandWork/Uploads/Products/" + ProductID);
                    int Count = 0;
                    NewProduct.ProductImages = new List<ProductImage>();
                    foreach (HttpPostedFileBase item in images)
                    {
                        ProductImage NewImage = new ProductImage();
                        string Path = Server.MapPath("/Uploads/Products/" + ProductID + "/");//dosya yolu
                        item.SaveAs(Path + Count + ".jpg");//image ismi                 
                        NewImage.ImageURL = "/Uploads/Products/"+ProductID+"/" + Count + ".jpg";            
                        NewProduct.ProductImages.Add(NewImage);
                        Count++;
                    }
                    NewProduct.HasPhoto = true;
                    _uw.ProductRepo.Edit(NewProduct);
                    _uw.Complete();
                }

                return RedirectToAction("MyProducts", "Member");
            }
            return RedirectToAction("MyProducts", "Member");
        }
        public ActionResult DeleteProduct(int ProductID)
        {

            _uw.ProductRepo.Delete(ProductID);
            _uw.Complete();

            return RedirectToAction("MyProducts", "Member");
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
        public ActionResult EditProduct(Product NewProduct, HttpPostedFileBase[] images,int id)
        {//burası bitmedi resimler gelmiyor

            ViewBag.Categories = _uw.CategoryRepo.GetAll().Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = (x.ID).ToString()
            });
            Product OldProduct = _uw.ProductRepo.GetOne(id);
            OldProduct.CategoryID = NewProduct.CategoryID;
            OldProduct.Content = NewProduct.Content;
            OldProduct.Price = NewProduct.Price;
            OldProduct.ProductName = NewProduct.ProductName;
            OldProduct.StockCount = NewProduct.StockCount;
         
            if (images != null && !images.Any(x=>x==null))
            {
                int Count = _uw.Db.ProductImages.OrderByDescending(x => x.ID).Select(x => x.ID).FirstOrDefault();
                foreach (HttpPostedFileBase item in images)
                {
                    ProductImage NewImage = new ProductImage();
                    string Path = Server.MapPath("/Uploads/Products/" +id + "/");//dosya yolu
                    item.SaveAs(Path + (Count + 1) + ".jpg");//image ismi                
                    NewImage.ImageURL = "/Uploads/Products/" + id + "/" + (Count+1) + ".jpg";
                    OldProduct.ProductImages.Add(NewImage);
                    Count++;
                }
            }
            _uw.Db.Entry(OldProduct).State = System.Data.Entity.EntityState.Modified;
            _uw.Complete();
            return RedirectToAction("MyProducts", "Member");
        }
        
        public ActionResult ProductDetail(int id)
        {
           //?
            Product product = _uw.ProductRepo.GetOne(id);
            return View(product);
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
        public JsonResult AddLike(int id)//beğenilen ürün id geliyor
        {
            Member member = User.GetMember(_uw);
            if (member.LikeModel == null)
                member.LikeModel = new LikeModel();

            bool DidLike = member.LikeModel.ProductLiked.Any(x => x.ID == id);
            if (DidLike == false)
            {
                Product product = _uw.ProductRepo.GetOne(id);
                product.LikeCount = (product.LikeCount) + 1;
                member.LikeModel.ProductLiked.Add(product);
                _uw.Db.Entry(member).State = System.Data.Entity.EntityState.Modified;
                _uw.ProductRepo.Edit(product);
                _uw.Complete();
                return Json(true);//urunu hiç beğenmediyse bir kere beğenebilir begeni dolucak
            }
            else
            {
                Product product = _uw.ProductRepo.GetOne(id);
                product.LikeCount = (product.LikeCount) - 1;
                member.LikeModel.ProductLiked.Remove(product);
                _uw.Db.Entry(member).State = System.Data.Entity.EntityState.Modified;
                _uw.ProductRepo.Edit(product);
                _uw.Complete();
                return Json(false);//urunu beğenmişse ve tekrar basarsa begeniye ozaman begenisini kaldırıyor
            }              
        }
        public JsonResult AddDisLike(int id)
        {
            Product product = _uw.ProductRepo.GetOne(id);
            product.DisLikeCount = product.DisLikeCount + 1;
            _uw.ProductRepo.Edit(product);
            _uw.Complete();
            return Json(true);
        }

    }
}
