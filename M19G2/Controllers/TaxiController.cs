using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using M19G2.Models;
using M19G2.IBLL;

namespace M19G2.Controllers
{
    [Authorize(Roles = "Delivery Service Staff")]
    public class TaxiController : BaseController
    {
        private readonly IDeliveryService deliveryService;
        private readonly IOrdersService ordersService;
        private readonly IUserService userService;
        private readonly IDeliveryAutomation deliveryAutomation;

        public TaxiController(IDeliveryService deliveryService, IOrdersService ordersService, IUserService userService, IDeliveryAutomation deliveryAutomation)
        {
            this.deliveryAutomation = deliveryAutomation;
            this.deliveryService = deliveryService;
            this.ordersService = ordersService;
            this.userService = userService;
        }

        // GET: Taxi
        public ActionResult Index()
        {
            List<DeliveryViewModel> model = deliveryAutomation.GetOrdersForStaff(CurrentUser.Id).Select(x => {
                var order = ordersService.GetOrderById(x);
                return new DeliveryViewModel()
                {
                    OrderID = order.OrderID,
                    AddressName = userService.GetUserAddress(order.AddressID).Name,
                    ETA = order.ETA
                };
            }).ToList();

            return View(model);
        }

        public EmptyResult SetETA(int orderId, int eta)
        {
            deliveryService.SetEstimatedArivalTime(orderId, eta);
            return new EmptyResult();
        }

        public EmptyResult MarkDelivered(int orderId)
        {
            deliveryService.MarkAsDelivered(orderId, true);
            deliveryAutomation.CompleteShipment(orderId, CurrentUser.Id);
            return new EmptyResult();
        }
    }
}