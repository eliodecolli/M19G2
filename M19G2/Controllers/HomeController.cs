using System.Web.Mvc;
using System;
using System.Text;
using M19G2.IBLL;
using M19G2.Models;
using M19G2.Common.Exceptions;

namespace M19G2.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IDishService dishService;
        private readonly IIngredientService ingredientService;
        private readonly IOrdersService ordersService;
        private readonly IDeliveryService deliveryService;

        public HomeController(IDishService dishService, IIngredientService ingredientService, IOrdersService orders, IDeliveryService delivery)
        {
            ordersService = orders;
            deliveryService = delivery;
            this.dishService = dishService;
            this.ingredientService = ingredientService;
        }

        public ActionResult Index()
        {
            var dishes = dishService.GetActiveDishes();
            return View(dishes);
        }

        [Authorize(Roles = "Client")]
        public ActionResult Orders()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}