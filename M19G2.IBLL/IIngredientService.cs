using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using M19G2.DAL;
using M19G2.Models;

namespace M19G2.IBLL
{
    public interface IIngredientService
    {
        // Get stuff
        IngredientDto GetIngredientByID(int id);
        IngredientDto GetIngredientByName(string name);
        IEnumerable<IngredientDto> GetAllIngredients();

        // Add stuff
        int AddNewIngredient(IngredientDto i);

        // Remove stuff
        void RemoveIngredient(int id);

        // Update stuff
        void UpdateIngredient(IngredientDto ing);



    }
}
