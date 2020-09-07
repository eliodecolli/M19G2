using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using M19G2.Models;
using M19G2.IBLL;
using M19G2.SessionStorage;
using M19G2.Filters;
using System.IO;

namespace M19G2.Controllers
{
    public class DishController : BaseController
    {
        private readonly IDishService dishService;
        private readonly ISessionStorage sessionStorage;

        public DishController(IDishService dishService, ISessionStorage sessionStorage)
        {
            this.dishService = dishService;
            this.sessionStorage = sessionStorage;
        }

        [Authorize(Roles = "Client")]
        public ActionResult Index()
        {
            var cc = dishService.GetActiveDishes() as List<DishDto>;

            return View(new DishesViewModel() { Dishes = cc });
        }

        [HttpGet]
        public FileResult ShowImage(int dishId)
        {
            var dishesImgs = dishService.GetImagesForDish(dishId);
            ImageDto img;

            if (dishesImgs.Count > 0)
                img = dishesImgs.ElementAt(0);  // first image 
            else
                img = dishService.GetImageById(1);  // default image

            return File(img.Data, "image");
        }

        [HttpGet]
        public FileResult ShowDishImage(int imageId)
        {
            return File(dishService.GetImageById(imageId).Data, "image");
        }

        [Authorize(Roles = "Chef")]
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase uploadedImage, int dishId)
        {
            using (MemoryStream memsr = new MemoryStream())
            {
                uploadedImage.InputStream.CopyTo(memsr);
                int imageId;
                dishService.UploadImage(memsr.ToArray(), dishId, out imageId);
            }
            return RedirectToAction("EditDish", "KitchenChef", new { id = dishId });
        }

        [Authorize]
        public ActionResult ViewDish(int id)
        {
            var dish = dishService.GetDishByID(id);

            return View("ViewDish", new ViewDishModel() { Dish = dish });
        }

        [Authorize(Roles = "Chef")]
        [HttpGet]
        public EmptyResult RemoveImage(int imageId)
        {
            dishService.RemoveImage(imageId);
            return new EmptyResult();
        }

        [Authorize(Roles = "Client")]
        [HttpGet]
        public ActionResult RenderDishFilter()
        {
            DishFilterViewModel model = new DishFilterViewModel
            {
                DishTypes = dishService.GetAllDishTypes().Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.Name }).ToList()
            };

            return PartialView("_DishesFilter", model);
        }

        [Authorize(Roles = "Client")]
        [HttpGet]
        public ActionResult Filter(string dishFilterName, string dishFilterTypeName, double? maxPrice, double? minPrice, string[] ingredients, bool? clearFilters)
        {
            var shit = new DishesViewModel()
            {
                Dishes = new List<DishDto>(),
                Types = new List<DishTypeDto>()
            };

            var filter = new Filter<DishDto>(dishService.GetActiveDishes());

           if(!clearFilters.HasValue || !clearFilters.Value)
            {
                if (!string.IsNullOrEmpty(dishFilterName))
                    filter.FilterDefinitions.Add(new FilterDefinition<DishDto>(x => x.Name.ToLower().Contains(dishFilterName.ToLower())));
                if (!string.IsNullOrEmpty(dishFilterTypeName))
                    filter.FilterDefinitions.Add(new FilterDefinition<DishDto>(x => x.DishType == int.Parse(dishFilterTypeName)));
                if (minPrice.HasValue)
                    filter.FilterDefinitions.Add(new FilterDefinition<DishDto>(x => x.Price >= (decimal)minPrice.Value));
                if (maxPrice.HasValue)
                    filter.FilterDefinitions.Add(new FilterDefinition<DishDto>(x => x.Price <= (decimal)maxPrice.Value));
                if (ingredients != null && ingredients.Length > 0)
                {
                    foreach (var ingredient in ingredients)
                        filter.FilterDefinitions.Add(new FilterDefinition<DishDto>((x) =>
                        {
                            foreach (var v in x.Ingredients)
                                if (v.Name == ingredient)
                                    return true;
                            return false;
                        }));  // huh this might take a bit..
                }
            }

            shit.Dishes = filter.ApplyFilters().ToList();

            return PartialView("_ListDishes", shit);
        }

        [Authorize(Roles = "Client")]
        [HttpGet]
        public ActionResult Remove(int id)
        {
            if (sessionStorage.IsOrderReady())
            {
                sessionStorage.GetCurrentOrder().CurrentOrder.DishesIDs.RemoveAll(x => x == id);
            }
            var dishes = new List<DishDto>();
            sessionStorage.GetCurrentOrder().CurrentOrder.DishesIDs.ForEach(did => dishes.Add(dishService.GetDishByID(did)));

            var orderReviewModel = new CartViewModel()
            {
                CurrentOrder = sessionStorage.GetCurrentOrder().CurrentOrder,
                Dishes = dishes
            };

            return PartialView("_cartListView", orderReviewModel);
        }

        [Authorize(Roles = "Client")]
        [HttpGet]
        public ActionResult Purchase(int id)
        {
            if (!sessionStorage.IsOrderReady())
                sessionStorage.ResetOrder(CurrentUser.Id);

            var dish = dishService.GetDishByID(id);
            sessionStorage.GetCurrentOrder().CurrentOrder.DishesIDs.Add(id);

            var dishes = new List<DishDto>();
            sessionStorage.GetCurrentOrder().CurrentOrder.DishesIDs.ForEach(did => {
                dishes.Add(dishService.GetDishByID(did));
            });

            var orderReviewModel = new CartViewModel()
            {
                CurrentOrder = sessionStorage.GetCurrentOrder().CurrentOrder,
                Dishes = dishes,
                Quantities = sessionStorage.GetCurrentOrder().Quantities
            };
            orderReviewModel.CalledFromNav = false;

            return PartialView("_cartReview", orderReviewModel);
        }
    }
}