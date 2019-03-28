using BLL;
using Entity;
using HandWork.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HandWork.Controllers
{
    public class OrderController : Controller
    {
        UnitOfWork _uw = new UnitOfWork();
        // GET: Order
        public ActionResult MyOrders()
        {
            Member member = User.GetMember(_uw);
            List<Order> orders = _uw.Db.Orders.Where(x => x.Member.Id == member.Id).ToList();             
            return View(orders);
        }
        public ActionResult OrderDetail(int id)
        {
            Order order = _uw.OrderRepo.GetOne(id);
            return View(order);
        }
    }
}