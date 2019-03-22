using BLL;
using Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HandWork.Controllers
{
    public class BasketController : Controller
    {
        UnitOfWork _uw = new UnitOfWork();
        // GET: Basket
   
        public ActionResult IndexBasket()//direk sepete basarsa
        {
         
            if (User.Identity.IsAuthenticated)
            {
                string MemberID = User.Identity.GetUserId();
                Member Member = _uw.Db.Users.Find(MemberID);
                if (Member.Basket == null)
                {
                    Member.Basket = new Basket();
                    Member.Basket.ProductItems = new List<ProductItem>();
                    ViewBag.ProductItemList = Member.Basket.ProductItems;
                    _uw.Db.Entry(Member).State = System.Data.Entity.EntityState.Modified;
                    _uw.Complete();
                    return View(Member.Basket);
                }
                else
                {                 
                    ViewBag.ProductItemList = Member.Basket.ProductItems;
                    return View(Member.Basket);
                }
            }
            else
                return RedirectToAction("Index", "Home", new { error = "sepetinize girebilmeniz için önce giriş yapmalısınız." });
            
        }
        public JsonResult DeleteProductItem(int id)//ürün item id
        {
            string MemberID = User.Identity.GetUserId();
            Member member = _uw.Db.Users.Find(MemberID);         
            foreach (var item in member.Basket.ProductItems)
            {
                if (item.ID==id)
                    member.Basket.ProductItems.Remove(item);
            }
            return Json(true);
          
        }
        public JsonResult IncreaseCount(int id)//ürünitem id si gelecek
        {
            string MemberID = User.Identity.GetUserId();
            Member member = _uw.Db.Users.Find(MemberID);          
            foreach (var item in member.Basket.ProductItems)
            {
                if (item.ID==id)
                {
                    item.ItemCount++;
                    _uw.Db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    _uw.Complete();
                }  
                  
            }
            return Json(true);

        }
        public JsonResult DecreaseCount(int id)//ürünitem id si gelecek
        {
            string MemberID = User.Identity.GetUserId();
            Member member = _uw.Db.Users.Find(MemberID);
            foreach (var item in member.Basket.ProductItems)
            {
                if (item.ID ==id)
                {
                    item.ItemCount--;
                    _uw.Db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    _uw.Complete();
                }

            }
            return Json(true);

        }
        public ActionResult AddToBasket(int id)//ürün id si
        {
            
            string MemberID = User.Identity.GetUserId();
            Member Member = _uw.Db.Users.Find(MemberID);
            Product product = _uw.ProductRepo.GetOne(id);
            ProductItem productItem = new ProductItem(); 

            productItem.Product = product;
            if (Member.Basket == null)
            {
                Member.Basket = new Basket();
                Member.Basket.ProductItems = new List<ProductItem>();
                Member.Basket.ProductItems.Add(productItem);
                _uw.Db.Entry(Member).State = System.Data.Entity.EntityState.Modified;
                _uw.Complete();
            }
            else if (Member.Basket.ProductItems == null)
            {
                Member.Basket.ProductItems = new List<ProductItem>();
                Member.Basket.ProductItems.Add(productItem);
                _uw.Db.Entry(Member).State = System.Data.Entity.EntityState.Modified;
                _uw.Complete();
            }
            else
            {
                foreach (var item in Member.Basket.ProductItems)
                {
                    if (item.Product.ID == id)
                    {
                        item.ItemCount++;
                        _uw.Db.Entry(productItem).State = System.Data.Entity.EntityState.Modified;
                        _uw.Complete();
                       
                    }
                }
            }
                return RedirectToAction("IndexBasket");
            }

        }
    }
