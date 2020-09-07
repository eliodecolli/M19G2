using System.Collections.Generic;
using System.Data;
using System.Linq;
using M19G2.DAL.Entities;
using M19G2.DAL;
using M19G2.IBLL;
using log4net;
using Log = M19G2.Common.Logging.CustomLogger;
using M19G2.Models;
using System;
using System.Linq.Expressions;
using M19G2.Common.Expressions;
using SysImg = System.Drawing.Image;
using System.IO;

namespace M19G2.BLL
{
    public class DishesService : IDishService
    {
        //POCO generations
        #region StaticConvert
        public Dish NewFromPOCO(DishDto dish)
        {
            Dish d = new Dish()
            {
                CreatedOn = dish.CreatedOn,
                Description = dish.Description,
                IsActive = dish.IsActive,
                TypeID = dish.DishType,
                Name = dish.Name,
                Price = (decimal)dish.Price,
                Orders = new List<Order>(),
                Ingredients = new List<Ingredient>(),
                Images = new List<Image>()
            };
            if (dish.Ingredients.Count > 0)
            {
                foreach (var v in dish.Ingredients)
                {
                    if (v == null)
                        continue;
                    var ing = unitOfWork.IngredientsRepository.GetByID(v.ID);
                    if (ing != null)
                        d.Ingredients.Add(ing);
                }
            }
            if(dish.ImagesId != null && dish.ImagesId.Count > 0)
            {
                dish.ImagesId.ForEach(x => {
                    var img = unitOfWork.ImagesRepository.GetByID(x);
                    if (img != null)
                        d.Images.Add(img);
                });
            }
            return d;
        }

        public Dish FromPOCO(DishDto dish)
        {
            Dish d = new DAL.Entities.Dish()
            {
                ID = dish.DishID,
                CreatedOn = dish.CreatedOn,
                Description = dish.Description,
                IsActive = dish.IsActive,
                TypeID = dish.DishType,
                Name = dish.Name,
                Price = (decimal)dish.Price,
                Orders = new List<Order>(),
                Ingredients = new List<Ingredient>(),
                Images = new List<Image>()
            };
            if (dish.Ingredients.Count > 0)
            {
                foreach (var v in dish.Ingredients)
                {
                    if (v == null)
                        continue;
                    var ing = unitOfWork.IngredientsRepository.GetByID(v.ID);
                    if (ing != null)
                        d.Ingredients.Add(ing);
                }
            }
            if (dish.ImagesId.Count > 0)
            {
                dish.ImagesId.ForEach(x => {
                    var img = unitOfWork.ImagesRepository.GetByID(x);
                    if (img != null)
                        d.Images.Add(img);
                });
            }
            return d;
        }

        public DishDto FromEF(Dish d)
        {
            if (d == null)
                return null;
            var retval = new DishDto()
            {
                DishID = d.ID,
                Ingredients = new List<IngredientDto>(),
                IsActive = d.IsActive,
                Price = d.Price,
                CreatedOn = d.CreatedOn,
                Description = d.Description,
                DishType = d.TypeID,
                DishTypeName = d.Type.Name,
                Name = d.Name,
                ImagesId = d.Images.Select(x => x.ImageID).ToList()
            };
            retval.Ingredients.AddRange(GetIngredientsForDish(d.ID));
            return retval;
        }

        public DishTypeDto DTFromEF(DishType type)
        {
            return new DishTypeDto() { Description = type.Description, ID = type.ID, Name = type.Name };
        }

        public DishType DTNewFromPOCO(DishTypeDto type)
        {
            return new DishType() { Description = type.Description, Name = type.Name, Dishes = new List<Dish>() };
        }

        public DishType DTFromPOCO(DishTypeDto type)
        {
            return new DishType() { ID = type.ID, Description = type.Description, Name = type.Name };
        }

        #endregion

        private readonly UnitOfWork unitOfWork;

        public DishesService(UnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;

        public void AddNewIngredient(int dishId, int ingredientId)
        {
            var d = unitOfWork.DishesRepository.GetByID(dishId);
            if (d != null)
            {
                Log.LogInfoFormat("Trying to add ingredient with ID {0} to {1}", new object[] { d.Name, ingredientId });
                var i = unitOfWork.IngredientsRepository.GetByID(ingredientId);
                if (i != null)
                {
                    d.Ingredients.Add(i);
                    unitOfWork.Save();
                    Log.LogInfo("The ingredient has been added :)");
                }
                else
                {
                    Log.LogError("The specified ingredient ID is invalid.");
                }
            }
        }

        public int CreateNewDish(DishDto dish)
        {
            var created = NewFromPOCO(dish);
            created = unitOfWork.DishesRepository.Insert(created);
            unitOfWork.Save();

            Log.LogInfoFormat("Created dish \"{0}\" with ID {1}", new object[] { created.Name, created.ID });

            return created.ID;
        }

        /// <summary>
        /// CAN RETURN A LIST WITH NO ELEMENTS
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ICollection<IngredientDto> GetIngredientsForDish(int id)
        {
            List<IngredientDto> retval = new List<IngredientDto>();
            var d = unitOfWork.DishesRepository.GetByID(id);
            if (d != null)
            {
                foreach (var v in d.Ingredients)
                    retval.Add(new IngredientDto() { ID = v.ID, Name = v.Name });
            }
            return retval;
        }

        public void RemoveIngredient(int dishId, int ingredientId)
        {
            var d = unitOfWork.DishesRepository.GetByID(dishId);
            if (d != null)
            {
                //Log.InfoFormat("Removing ingredient {0} from {1}.", new object[] { d.Name, ingredientId });
                var i = d.Ingredients.FirstOrDefault(x => x.ID == ingredientId);
                if (i != null)
                {
                    d.Ingredients.Remove(i);
                    unitOfWork.Save();
                    Log.LogInfo("Ingredient has been removed :(");
                }
                else
                {
                    Log.LogError("Couldn't find ingredient with that ID");
                }
            }
        }

        /// <summary>
        /// CAN RETURN NULL
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DishDto GetDishByID(int id)
        {
            DishDto retval = null;
            retval = FromEF(unitOfWork.DishesRepository.GetByID(id));
            return retval;
        }

        /// <summary>
        /// CAN RETURN A LIST WITH NO ELEMENTS
        /// </summary>
        /// <param name="queryName"></param>
        /// <returns></returns>
        public ICollection<DishDto> GetDishesByName(string queryName)
        {
            List<DishDto> retvals = new List<DishDto>();
            var d = unitOfWork.DishesRepository.Get(x => x.Name.ToLower().Contains(queryName.ToLower())).Select(x => FromEF(x));  // Get ska nevoj per try, whatever..
            retvals.AddRange(d);
            return retvals;
        }

        /// <summary>
        /// CAN RETURN A LIST WITH NO ELEMENTS
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public ICollection<DishDto> GetDishesByType(int typeId)
        {
            return unitOfWork.DishesRepository.Get(x => x.TypeID == typeId).Select(d => FromEF(d)).ToList();
        }


        public void RemoveDish(int id)
        {
            var d = unitOfWork.DishesRepository.GetByID(id);
            if (d != null)
            {
                unitOfWork.DishesRepository.Delete(d);
                unitOfWork.Save();
                Log.LogInfoFormat("Removing dish {0}", new object[] { d.Name });
            }

            else
            {
                Log.LogInfo($"Failed to remove dish.\nCause: Dish with id = {id} was not found");
            }
        }

        public int CreateNewDishType(DishTypeDto type)
        {
            Log.LogInfoFormat("Creating dish type {0}", new object[] { type.Name });
            var dt = unitOfWork.DishTypeRepository.Insert(DTNewFromPOCO(type));
            unitOfWork.Save();

            return dt.ID;
        }

        public void UpdateDishType(DishTypeDto toBeUpdated)
        {
            //Log.InfoFormat("Updating dish type \"{0}\"", new object[] { toBeUpdated.Name });
            unitOfWork.DishTypeRepository.Update(DTFromPOCO(toBeUpdated));

            unitOfWork.Save();
        }
        //Edit dish
        public void UpdateDish(DishDto dishDto)
        {
            var dish = unitOfWork.DishesRepository.GetByID(dishDto.DishID);
            dish.Name = dishDto.Name;
            dish.Description = dishDto.Description;
            dish.Price = dishDto.Price;
            dish.TypeID = dishDto.DishType;

            var dishIngList = new List<Ingredient>();
            foreach (var ing in dishDto.Ingredients)
            {
                var ingredient = unitOfWork.IngredientsRepository.GetByID(ing.ID);
                dishIngList.Add(ingredient);
            }
            dish.Ingredients.Clear();
            dish.Ingredients = dishIngList;

            unitOfWork.DishesRepository.Update(dish);
            unitOfWork.Save();
        }

        public void DisableDish(int id)
        {
            unitOfWork.DishesRepository.GetByID(id).IsActive = false;

            unitOfWork.Save();
        }

        public void EnableDish(int id)
        {
            unitOfWork.DishesRepository.GetByID(id).IsActive = true;
            unitOfWork.Save();
        }

        public void RemoveDishType(int id)
        {
            var dt = unitOfWork.DishTypeRepository.GetByID(id);
            if (dt != null)
            {
                unitOfWork.DishTypeRepository.Delete(dt);
                unitOfWork.Save();
                Log.LogInfoFormat("Removed dish type \"{0}\"", new object[] { dt.Name });
            }
        }

        /// <summary>
        /// CAN RETURN NULL
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DishTypeDto GetDishTypeByID(int id)
        {
            return DTFromEF(unitOfWork.DishTypeRepository.GetByID(id));
        }

        public ICollection<DishDto> GetActiveDishes()
        {
            return unitOfWork.DishesRepository.Get(x => x.IsActive).Select(dish => FromEF(dish)).ToList();
        }

        public ICollection<DishDto> GetAllDishes()
        {
            var retvals = new List<DishDto>();
            retvals.AddRange(unitOfWork.DishesRepository.Get().Select(x => FromEF(x)));

            return retvals;
        }

        public ICollection<DishTypeDto> GetAllDishTypes()
        {
            return unitOfWork.DishTypeRepository.Get().Select(x => DTFromEF(x)).ToList();
        }

        public DishTypeDto GetDishTypeByName(string dTypeName)
        {
            return unitOfWork.DishTypeRepository.Get(x => x.Name == dTypeName).Select(x => DTFromEF(x)).FirstOrDefault();
        }

        public List<DishDto> FilterDishes(string dishFilterName, string dishFilterTypeName, double? maxPrice, double? minPrice, string[] ingredients)
        {
            var filter = new Filter<Dish>();

            if (!string.IsNullOrEmpty(dishFilterName))
                filter.AddFilter(x => x.Name.ToLower().Contains(dishFilterName.ToLower()));
            if (!string.IsNullOrEmpty(dishFilterTypeName))
                filter.AddFilter(x => x.TypeID == int.Parse(dishFilterTypeName));
            if (minPrice.HasValue)
                filter.AddFilter(x => x.Price >= (decimal)minPrice.Value);
            if (maxPrice.HasValue)
                filter.AddFilter(x => x.Price <= (decimal)maxPrice.Value);

            filter.AddFilter(dish => dish.Ingredients.Select(ingredient => ingredient.Name).Intersect(ingredients).Any());

            return unitOfWork.DishesRepository.Get(filter.MegaFilter).Select(FromEF).ToList();
        }

        public ICollection<ImageDto> GetImagesForDish(int dishId)
        {
            return unitOfWork.DishesRepository.GetByID(dishId).Images.Select(x =>
            {
                return new ImageDto() { ID = x.ImageID, Data = x.ImageData };
            }).ToList();
        }

        public ImageDto GetImageById(int id)
        {
            var img = unitOfWork.ImagesRepository.GetByID(id);

            return new ImageDto() { ID = img.ImageID, Data = img.ImageData };
        }

        public bool UploadImage(byte[] data, int dishId, out int imageId)
        {
            using (MemoryStream memsr = new MemoryStream(data))
            {
                var mm = SysImg.FromStream(memsr);
                if (mm.Height > 500 || mm.Width > 500)
                {
                    Log.LogError($"Cannot upload image with size {mm.Width}x{mm.Height} maximum size is 500x500");
                    imageId = -1;
                    return false;
                }
            }
            var img = new Image() { DishID = dishId, ImageData = data };
            img = unitOfWork.ImagesRepository.Insert(img);
            unitOfWork.Save();
            imageId = img.ImageID;
            Log.LogInfo($"Uploaded image for dish with ID {dishId}");
            return true;
        }

        public void RemoveImage(int imageId)
        {
            var img = unitOfWork.ImagesRepository.GetByID(imageId);
            unitOfWork.ImagesRepository.Delete(img);

            unitOfWork.Save();
        }
    }
}
