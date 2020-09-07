using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using M19G2.IBLL;
using M19G2.Models;
using M19G2.SessionStorage;

namespace M19G2.Controllers
{
    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IOrdersService ordersService;
        private readonly IDishService dishService;
        private readonly ISessionStorage sessionStorage;
        private readonly IOrderQueueService queueService;

        public OrdersController(IOrdersService ordersService, IDishService dishService, ISessionStorage sessionStorage,
            IOrderQueueService queueService)
        {
            this.queueService = queueService;
            this.ordersService = ordersService;
            this.dishService = dishService;
            this.sessionStorage = sessionStorage;
        }

        private OrderViewModel GetOrders(bool delivered)
        {
            var o = ordersService.ListOrdersForUser(CurrentUser.Id).Where(x => x.IsDelivered == delivered && x.Active).ToList();
            var orders = new Dictionary<int, string[]>();
            var orderDates = new Dictionary<int, string>();
            o.ForEach(order =>
            {
                var dNames = new List<string>();
                order.DishesIDs.ForEach(dish =>
                {
                    dNames.Add(dishService.GetDishByID(dish).Name);
                });
                orders.Add(order.OrderID, dNames.ToArray());
                orderDates.Add(order.OrderID, order.Created.ToString("dd/MM/yyy"));
            });

            return new OrderViewModel() { Orders = orders, OrderDates = orderDates };
        }

        [Authorize(Roles = "Kitched Staff")]
        [HttpGet]
        public ActionResult PeekDishes(int orderId)
        {
            List<string> model = new List<string>();

            ordersService.GetOrderById(orderId).DishesIDs.ForEach(x => model.Add(dishService.GetDishByID(x).Name));

            return PartialView("_listOrderDishes", model);
        }

        [HttpGet]
        public ActionResult GetActiveOrders()
        {
            return PartialView("_listActiveOrders", GetOrders(false));
        }

        [HttpGet]
        public ActionResult GetOldOrders()
        {
           return PartialView("_listOldOrders", GetOrders(true));
        }

        public ActionResult OrderAgain(int id)
        {
            var o = ordersService.GetOrderById(id);
            sessionStorage.SetOrder(new StorageOrder() { CurrentOrder = o });

            return RedirectToAction("Index", "Cart", new { completedPurchase = false });
        }

        [HttpGet]
        public ActionResult Cancel(int id)
        {
            var cc = DateTime.Now - ordersService.GetOrderById(id).Created;
            string Message = "";
            if (cc.Minutes <= 10)
            {
                ordersService.CancelOrder(id);
                Message = "That order has been canceled sir :)";
            }
            else
                Message = "You cannot cancel orders older than 10 minutes.";

            return Content(Message);
        }

        [HttpGet]
        public ActionResult Status(int orderId)
        {
            return Content(queueService.OrderStatus(orderId));
        }

        public EmptyResult SetQuantity(int dishId, int quantity)
        {
            /// could've been done better but who cares am i rite?
            sessionStorage.GetCurrentOrder().CurrentOrder.DishesIDs.RemoveAll(x => x == dishId);
            for(int i = 0; i < quantity; i++)
                sessionStorage.GetCurrentOrder().CurrentOrder.DishesIDs.Add(dishId);

            return new EmptyResult();
        }
    }
}