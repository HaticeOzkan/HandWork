﻿using BLL;
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
            Member Member = User.GetMember(_uw);

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
            {
                TempData["Error"] = "Sepete gidebilmek için giriş yapılmalıdır";
                return RedirectToAction("Index", "Home");
            }



        }
        public JsonResult DeleteProductItem(int id)//ürün item id
        {
            Member Member = User.GetMember(_uw);
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
            if (User.Identity.IsAuthenticated)
            {
                Member Member = User.GetMember(_uw);
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
            else
            {
                TempData["Error"] = "Ürün Satın Almadan Önce Giriş Yapınız";
                return RedirectToAction("Index","Home");
            }
          
        }
        public JsonResult UpdateProductItemCount(int ItemID, int CurrentCount)
        {
            Member member = User.GetMember(_uw);
            ProductItem productItem = _uw.Db.ProductItems.Find(ItemID);
            productItem.ItemCount = CurrentCount;
            _uw.Db.Entry(productItem).State = System.Data.Entity.EntityState.Modified;
            _uw.Complete();
            decimal CurrentPrice = productItem.TotalPrice;
            decimal TotalPrice= member.Basket.SubTotal;
            return Json(new { CurrentPrice, TotalPrice });

        }
        public ActionResult CheckOut()//sepet id si geliyor
        {          
            Member Member = User.GetMember(_uw);
            ViewBag.SubTotal = Member.Basket.SubTotal.ToString("C");
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
            Member Member = User.GetMember(_uw);
            Order order = new Order();
            order.Member = Member;
            order.OrderItems = new List<OrderItem>();
            order.IsPaid = isPaid;
            foreach (ProductItem item in Member.Basket.ProductItems)
            {
                Product product = item.Product;
                product.StockCount = product.StockCount + (item.ItemCount);
                _uw.ProductRepo.Edit(product);
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
            Member Member = User.GetMember(_uw);
            Member.Basket.ProductItems.Clear();
            List<ProductItem> productItems = _uw.Db.ProductItems.Where(x => x.Basket.ID == Member.Basket.ID).ToList();
            foreach (var item in productItems)
            {
                _uw.Db.ProductItems.Remove(item);
            }                       
            _uw.Complete();

        }

        [HttpPost]
        public ActionResult PayCreditCart(CreditCardPayment paymentModel)//kişi onaya bastıysa
        {
            if (ModelState.IsValid)
            {
                CreditCardPayment c1 =paymentModel;
                CreditCartService service = new CreditCartService();
                bool isPaid = service.MakePayment(c1);
                CreateOrder(isPaid);
                ResetShoppingCart();
                return RedirectToAction("Success", "Home");
            }else
            return RedirectToAction("Fail", "Home");           
        }
    }
}