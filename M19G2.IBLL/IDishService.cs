using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M19G2.Models;
using System.Linq.Expressions;

namespace M19G2.IBLL
{
    public interface IDishService
    {
        // Create
        int CreateNewDish(DishDto dish);

        // Update a dish
        void AddNewIngredient(int dishId, int ingredientId);
        void RemoveIngredient(int dishId, int ingredientId);

        // Get Dishes and details
        DishDto GetDishByID(int id);
        ICollection<DishDto> GetDishesByName(string queryName);
        ICollection<DishDto> GetDishesByType(int typeId);
        ICollection<IngredientDto> GetIngredientsForDish(int id);
        ICollection<DishDto> GetAllDishes();
        DishTypeDto GetDishTypeByName(string dTypeName);
        ICollection<DishDto> GetActiveDishes();

        // Remove dishesh and suff
        void RemoveDish(int id);

        // ----------------   Dish types ---------------------
        int CreateNewDishType(DishTypeDto type);
        void UpdateDishType(DishTypeDto toBeUpdated);
        void UpdateDish(DishDto id);
        void DisableDish(int id);
        void EnableDish(int id);
        void RemoveDishType(int id);
        DishTypeDto GetDishTypeByID(int id);
        ICollection<DishTypeDto> GetAllDishTypes();

        ImageDto GetImageById(int id);
        ICollection<ImageDto> GetImagesForDish(int dishId);

        void RemoveImage(int imageId);

        bool UploadImage(byte[] data, int dishId, out int imageId);

        List<DishDto> FilterDishes(string dishFilterName, string dishFilterTypeName, double? maxPrice, double? minPrice, string[] ingredients);
    }
}
