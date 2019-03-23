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
            string MemberID = User.Identity.GetUserId();
            Member Member = _uw.Db.Users.Find(MemberID);

            if (User.Identity.IsAuthenticated)
            {
               
                if (Member.Basket == null)
                {
                    Member.Basket = new Basket();
                    Member.Basket.ProductItems = new List<ProductItem>();
                    _uw.Db.Entry(Member).State = System.Data.Entity.EntityState.Modified;
                    _uw.Complete();
                    return View(Member.Basket);
                }
                else
                {
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
            foreach (var item in member.Basket.ProductItems.ToList())
            {
                if (item.ID == id)
                {
                    //member.Basket.ProductItems.Remove(item);                    
                    ProductItem productItem = _uw.Db.ProductItems.Find(id);
                    //Product product = productItem.Product;
                    //product.ProductItems.Remove(productItem);
                    //_uw.Db.Entry(member).State = System.Data.Entity.EntityState.Modified;
                    //_uw.Db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                    _uw.Db.ProductItems.Remove(productItem);
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
            bool control = false;
            foreach (ProductItem item in Member.Basket.ProductItems)
            {
                if (item.Product.ID == id)
                {
                    ProductItem productItem = _uw.Db.ProductItems.Find(item.ID);
                    productItem.ItemCount++;
                    _uw.Db.Entry(productItem).State = System.Data.Entity.EntityState.Modified;
                    _uw.Complete();
                    control = true;

                }
            }
            if (control == false)
            {
                if (Member.Basket == null)
                {
                    Member.Basket = new Basket();
                    Member.Basket.ProductItems = new List<ProductItem>();
                }
                ProductItem ProductItem = new ProductItem();
                ProductItem.Product = product;
                ProductItem.Basket = Member.Basket;
                Member.Basket.ProductItems = new List<ProductItem>();
                product.ProductItems = new List<ProductItem>();
                product.ProductItems.Add(ProductItem);
                Member.Basket.ProductItems.Add(ProductItem);
                _uw.Db.ProductItems.Add(ProductItem);
                _uw.Complete();

            }

            return RedirectToAction("IndexBasket");
        }
        public JsonResult UpdateProductItemCount(int ItemID, int CurrentCount)
        {
            ProductItem productItem = _uw.Db.ProductItems.Find(ItemID);
            productItem.ItemCount = CurrentCount;
            _uw.Db.Entry(productItem).State = System.Data.Entity.EntityState.Modified;
            _uw.Complete();
            return Json(true);

        }
        public ActionResult CheckOut()//sepet id si geliyor
        {
            string MemberID = User.Identity.GetUserId();
            Member Member = _uw.Db.Users.Find(MemberID);
            ViewBag.SubTotal = Member.Basket.SubTotal;
            ViewBag.CartNo = Member.Basket.ID;
            return View();
        }
        [HttpPost]
        public ActionResult PayBankTransfer(int? approve)
        {
            if (approve.HasValue && approve.Value == 1)
            {
                BankTransferPayment p1 = new BankTransferPayment();
                p1.IsApproved = false;
                //p1.NameSurname = User.Identity.GetNameSurname();
                //p1.TC = User.Identity.GetTC();

                BankTransferService service = new BankTransferService();

                bool isPaid = service.MakePayment(p1);


                if (isPaid)
                {
                    //CreateOrder(isPaid);
                    //ResetShoppingCart();
                }


                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Checkout");
        }


    }
}