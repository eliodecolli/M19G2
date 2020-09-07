using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using M19G2.Models;
using M19G2.IBLL;

namespace M19G2.Controllers
{
    [Authorize]
    public class StatisticsController : BaseController
    {
        private readonly IStatisticsService statisticsService;
        private readonly IDishService dishService;

        public StatisticsController(IStatisticsService statisticsService, IDishService dishService)
        {
            this.dishService = dishService;
            this.statisticsService = statisticsService;
        }

        [HttpGet]
        public ActionResult GetTop()
        {
            var model = new StatisticsViewModel();
            model.Dishes = new List<DishDto>();

            model.Dishes = statisticsService.GetMostRequestedDishes(3).Select(x => dishService.GetDishByID(x)).ToList();

            return PartialView("_mostOrderedDishes", model);
        }
    }
}