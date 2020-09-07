using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using M19G2.IBLL;
using M19G2.Models;
using M19G2.SessionStorage;

using System.Threading;

namespace M19G2.Controllers
{
    [Authorize(Roles = "Client")]
    public class CartController : BaseController
    {
        private readonly ISessionStorage sessionStorage;
        private readonly IDishService dishService;
        private readonly IOrdersService ordersService;
        private readonly IUserService userService;
        private readonly IOrderQueueService queueService;

        public CartController(ISessionStorage ss, IDishService ds, IOrdersService ordersService, IUserService userService,
            IOrderQueueService qservice)
        {
            queueService = qservice;
            this.userService = userService;
            sessionStorage = ss;
            dishService = ds;
            this.ordersService = ordersService;
        }

        public ActionResult Index(bool? completedPurchase, int? oldId, string errorMessage)
        {
            var model = new CartViewModel();
            model.Dishes = new List<DishDto>();
            model.Quantities = new Dictionary<int, int>();

            if(!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.ErrorMessage = errorMessage;
                return View(model);
            }

            if (!completedPurchase.HasValue || (completedPurchase.HasValue && !completedPurchase.Value))
            {
                if (sessionStorage.IsOrderReady())
                {
                    var corder = sessionStorage.GetCurrentOrder();
                    corder.CurrentOrder.DishesIDs.ForEach(x => model.Dishes.Add(dishService.GetDishByID(x)));
                    model.CurrentOrder = corder.CurrentOrder;
                    model.CurrentOrder.Message = "";
                    model.Quantities = corder.Quantities;
                    model.UserAddresses = new List<SelectListItem>();
                    model.CalledFromNav = true;

                    userService.GetUserAddresses(CurrentUser.Id).ToList().ForEach(x => model.UserAddresses.Add(new SelectListItem() { Value = x.ID.ToString(), Text = x.Name }));
                }
                else
                {
                    sessionStorage.ResetOrder(CurrentUser.Id);
                    model.CurrentOrder = sessionStorage.GetCurrentOrder().CurrentOrder;
                    model.CurrentOrder.Message = "";
                }
                model.Completed = false;
            }
            else
            {
                if (completedPurchase.Value)
                {
                    ViewBag.OldId = oldId.Value;
                    model.Completed = true;
                    model.CurrentOrder = sessionStorage.GetCurrentOrder().CurrentOrder;
                }
            }

            return View(model);
        }


        [HttpGet]
        public ActionResult Remove(int id)
        {
            if (sessionStorage.IsOrderReady())
            {
                sessionStorage.GetCurrentOrder().CurrentOrder.DishesIDs.RemoveAll(x => x == id);
            }
            var dishes = new List<DishDto>();
            sessionStorage.GetCurrentOrder().CurrentOrder.DishesIDs.ForEach(did => {
                dishes.Add(dishService.GetDishByID(did));
            });

            var orderReviewModel = new CartViewModel()
            {
                CurrentOrder = sessionStorage.GetCurrentOrder().CurrentOrder,
                Dishes = dishes,
                CalledFromNav = true,
                UserAddresses = new List<SelectListItem>(),
                Quantities = sessionStorage.GetCurrentOrder().Quantities
            };

            userService.GetUserAddresses(CurrentUser.Id).ToList().ForEach(x => orderReviewModel.UserAddresses.Add(new SelectListItem() { Value = x.ID.ToString(), Text = x.Name }));

            return PartialView("_cartListView", orderReviewModel);
        }

        private void WaitThread(object id)
        {
            Thread.Sleep(1);
            queueService.PushOrder((int)id);
        }

        [HttpPost]
        public ActionResult CompleteCheckout(string extraDetails, string deliveryAddress)
        {
            OrderDto finalOrder = new OrderDto();
            finalOrder = sessionStorage.GetCurrentOrder().CompileOrder();
            finalOrder.Message = extraDetails;
            finalOrder.AddressID = int.Parse(deliveryAddress);

            bool canComplete = true;
            string errorDish = "";
            finalOrder.DishesIDs.ForEach(x =>
            {
                var dish = dishService.GetDishByID(x);
                if(!dish.IsActive)
                {
                    canComplete = false;
                    errorDish = dish.Name;
                }
            });
            if(!canComplete)
            {
                sessionStorage.ResetOrder(CurrentUser.Id);
                return RedirectToAction("Index", new { errorMessage = $"{errorDish} is not available anymore, your order cannot be completed." });
            }

            var id = ordersService.CreateNewOrder(finalOrder);
            //queueService.PushOrder(id);
            new Thread(WaitThread).Start(id);
            sessionStorage.ResetOrder(CurrentUser.Id);

            return RedirectToAction("Index", new { completedPurchase = true, oldId = id });
        }

        [HttpGet]
        public ActionResult GetCartNum()
        {
            if (!sessionStorage.IsOrderReady())
                return Content("Cart (0)");
            else
            {
                var numOfItems = sessionStorage.GetCurrentOrder().CurrentOrder.DishesIDs.Count;
                return Content($"Cart ({ numOfItems })");
            }
        }
    }
}