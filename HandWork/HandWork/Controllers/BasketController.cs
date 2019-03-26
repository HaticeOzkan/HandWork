using BLL;
using Entity;
using HandWork.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
            Member Member = User.GetMember();

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
            Member Member = User.GetMember();
            foreach (var item in Member.Basket.ProductItems.ToList())
            {
                if (item.ID == id)
                {                   
                    ProductItem productItem = _uw.Db.ProductItems.Find(id);
                    _uw.Db.ProductItems.Remove(productItem);
                    _uw.Complete();
                }
            }
            return Json(true);

        }
        public ActionResult AddToBasket(int id)//ürün id si
        {
            Member Member = User.GetMember();
            Product product = _uw.ProductRepo.GetOne(id);
            bool control = false;
            if (Member.Basket == null)
            {
                Member.Basket = new Basket();
                Member.Basket.ProductItems = new List<ProductItem>();
                product.ProductItems = new List<ProductItem>();
                _uw.BasketRepo.Add(Member.Basket);           
                _uw.Complete();

            }
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
                ProductItem ProductItem = new ProductItem();
                ProductItem.Product = product;
                ProductItem.Basket = Member.Basket;                
                product.ProductItems.Add(ProductItem);
                 Member.Basket.ProductItems.Add(ProductItem);
               _uw.Db.Entry(Member.Basket).State = System.Data.Entity.EntityState.Modified;
                _uw.Db.Entry(product).State = System.Data.Entity.EntityState.Modified;
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
           
            Member Member = User.GetMember();
            ViewBag.SubTotal = Member.Basket.SubTotal;
            ViewBag.BasketNo = Member.Basket.ID;
            return View();
        }
        [HttpPost]
        public ActionResult PayBankTransfer(int? approve)//kişi onaya bastıysa
        {
            BankTransferPayment p1 = new BankTransferPayment();
            p1.IsApproved = false;
            p1.NameSurname = User.Identity.GetNameSurname();
            BankTransferService service = new BankTransferService();
            bool isPaid = service.MakePayment(p1);         
            CreateOrder(isPaid);
            ResetShoppingCart();
            return RedirectToAction("Success", "Home");
  
        }

        private void CreateOrder(bool isPaid)
        {
            Member Member = User.GetMember();
            Order order = new Order();
            order.Member = Member;
            order.OrderItems = new List<OrderItem>();
            order.IsPaid = isPaid;
            foreach (ProductItem item in Member.Basket.ProductItems)
            {
                OrderItem OrderItem = new OrderItem();
                OrderItem.Product = item.Product;
                OrderItem.Order = order;
                OrderItem.ItemCount = item.ItemCount;
                order.OrderItems.Add(OrderItem);             
            }
            _uw.OrderRepo.Add(order);
            _uw.Complete();

        }

        private void ResetShoppingCart()
        {
            Member Member = User.GetMember();
            Member.Basket.ProductItems.Clear();
            _uw.Db.Entry(Member).State = System.Data.Entity.EntityState.Modified;
            _uw.Complete();

        }

        [HttpPost]
        public ActionResult PayCreditCart(int? approve)//kişi onaya bastıysa
        {
            CreditCardPayment c1 = new CreditCardPayment(); 
            c1.IsApproved = false;
            c1.NameSurname = User.Identity.GetNameSurname();
            CreditCartService service = new CreditCartService();
            bool isPaid = service.MakePayment(c1);
            CreateOrder(isPaid);
            ResetShoppingCart();
            return RedirectToAction("Success", "Home");           
        }
    }
}