using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using M19G2.IBLL;
using M19G2.Models;
using M19G2.ViewModels;

namespace M19G2.Controllers
{
    [Authorize(Roles = "Chef")]
    public class KitchenChefController : Controller
    {
        private readonly IDishService dishService;
        private readonly IIngredientService ingredientService;

        public KitchenChefController(IDishService dishService, IIngredientService ingredientService)
        {
            this.dishService = dishService;
            this.ingredientService = ingredientService;
        }

        // GET: KitchenChef
        public ActionResult Index()
        {
            var viewModel = new DishesViewModel
            {
                Dishes = new List<DishDto>(),
                Types = new List<DishTypeDto>()
            };

            viewModel.Dishes.AddRange(dishService.GetAllDishes().ToList());
            return View(viewModel);
        }

        //Display dish info
            public ActionResult ViewDish(int id)
            {
                DishDto dish = dishService.GetDishByID(id);

                //var e = ingredientService.GetIngredientByName("Tomato");

                var viewModel = new DishViewModel
                {
                    Description = dish.Description, Name = dish.Name, Price = dish.Price, ID = dish.DishID
                };

                return View(viewModel);
            }

            //why is this
        [HttpGet]
        public ActionResult AddDish()
        {
            try
            {
                var dishViewModel = new SaveDishViewModel()
                {
                    DishDto = new DishDto(),
                    Ingredients = ingredientService.GetAllIngredients().ToList(),
                    DishTypeDtos = dishService.GetAllDishTypes().ToList()
                };
                return View("SaveDishForm", dishViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public ActionResult EditDish(int id)
        {
            try
            {
                var dishInDb = dishService.GetDishByID(id);
                var ingredients = ingredientService.GetAllIngredients().ToList();
                foreach (var ing in ingredients)
                {
                    if (dishInDb.Ingredients.FirstOrDefault(ingredient => ingredient.ID == ing.ID) != null)
                    {
                        ing.IsSelected = true;
                    }
                }
                var dishViewModel = new SaveDishViewModel()
                {
                    DishDto = dishInDb,
                    DishTypeDtos = dishService.GetAllDishTypes().ToList(),
                    Ingredients = ingredients
                };
                return View("SaveDishForm", dishViewModel);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpPost]
        public ActionResult SaveDish(SaveDishViewModel dish)
        {
            if (!ModelState.IsValid)    
            {
                var dishViewModel = new SaveDishViewModel()
                {
                    DishDto = dish.DishDto,
                    Ingredients = ingredientService.GetAllIngredients().ToList(),
                    DishTypeDtos = dishService.GetAllDishTypes().ToList()
                };

                return View("SaveDishForm", dishViewModel);
            }
            try
            {
                if (dish.DishDto.DishID == 0)
                {
                    DishDto newDish = new DishDto()
                    {
                        Name = dish.DishDto.Name,
                        Description = dish.DishDto.Description,
                        CreatedOn = DateTime.Now,
                        Price = dish.DishDto.Price,
                        IsActive = true,
                        DishType = dish.DishDto.DishType,
                        Ingredients = dish.Ingredients.Where(ing => ing.IsSelected).ToList()
                    };
                    dishService.CreateNewDish(newDish);

                    return RedirectToAction("Index");
                }
                //edit dish
                DishDto editDishDto = new DishDto()
                {
                    DishID = dish.DishDto.DishID,
                    Name = dish.DishDto.Name,
                    Description = dish.DishDto.Description,
                    Price = dish.DishDto.Price,
                    DishType = dish.DishDto.DishType,
                    Ingredients = dish.Ingredients.Where(ing => ing.IsSelected).ToList(),
                };

                dishService.UpdateDish(editDishDto);
                return RedirectToAction("Index");
            }

            catch (NullReferenceException e)
            {
                return new HttpStatusCodeResult(500);
            }

        }

        [HttpPost]
        public ActionResult AddDishType(string dishTypeName)
        {
            try
            {
                var dishTypeDto = new DishTypeDto() { Name = dishTypeName, Description = dishTypeName};
                dishService.CreateNewDishType(dishTypeDto);
                return RedirectToAction("AddDish");
            }
            catch (Exception exception)
            {
                return new HttpStatusCodeResult(500);
            }
        }

        public ActionResult DisableDish(int id)
        {
            try
            {
                dishService.DisableDish(id);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                return new HttpStatusCodeResult(500);
            }
        }

        public ActionResult EnableDish(int id)
            {
                try
                {
                    dishService.EnableDish(id);
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    return new HttpStatusCodeResult(500);
                }
            }

        [HttpPost]
        public ActionResult AddIngredient(string ingredientName)
        {
            ingredientService.AddNewIngredient(new IngredientDto(){Name=ingredientName});

            return RedirectToAction("IngredientsIndex", "KitchenChef");
        }

        public ActionResult IngredientsIndex()
        {
            try
            {
            var viewModel = new IngredientsViewModel
            {
                IngredientDtos = ingredientService.GetAllIngredients().ToList()
            };

            return View(viewModel);

            }
            catch (Exception e)
            {
               return new HttpStatusCodeResult(500);
            }
        }
    }
}