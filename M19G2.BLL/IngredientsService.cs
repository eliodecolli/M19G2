using System.Linq;
using M19G2.IBLL;
using M19G2.DAL;
using M19G2.DAL.Entities;
using M19G2.Models;
using log4net;
using System;
using System.Collections.Generic;

namespace M19G2.BLL
{
    public class IngredientsService : IIngredientService
    {
        #region StaticConvert

        public static IngredientDto FromEF(Ingredient ing)
        {
            if (ing == null)
                return null;
            return new IngredientDto() { ID = ing.ID, Name = ing.Name };
        }

        public static Ingredient NewFromPOCO(IngredientDto ing)
        {
            return new Ingredient() { Name = ing.Name };
        }

        public static Ingredient FromPOCO(IngredientDto ing)
        {
            return new Ingredient() { ID = ing.ID, Name = ing.Name };
        }
        #endregion

        private readonly UnitOfWork unitOfWork;
        private readonly ILog Log = LogManager.GetLogger(Environment.MachineName);

        public IngredientsService(UnitOfWork unitOfWork) =>
            this.unitOfWork = unitOfWork;

        /// <summary>
        /// CAN RETURN NULL
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IngredientDto GetIngredientByID(int id)
        {
            IngredientDto retval = FromEF(unitOfWork.IngredientsRepository.GetByID(id));
            return retval;
        }


        public IEnumerable<IngredientDto> GetAllIngredients()
        {
            List<IngredientDto> ingsToReturn = new List<IngredientDto>();
            //Add all ings in DB to list
            ingsToReturn.AddRange(unitOfWork.IngredientsRepository.Get().Select(ing => FromEF(ing)).ToList());

            return ingsToReturn;
        }
        public int AddNewIngredient(IngredientDto i)
        {
            Log.InfoFormat("Adding new ingredient \"{0}\"", i.Name);
            var ing = unitOfWork.IngredientsRepository.Insert(NewFromPOCO(i));
            unitOfWork.Save();

            return ing.ID;
        }

        public void RemoveIngredient(int id)
        {
            unitOfWork.IngredientsRepository.Delete(unitOfWork.IngredientsRepository.GetByID(id));
            unitOfWork.Save();
        }

        /// <summary>
        /// CAN RETURN NULL
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IngredientDto GetIngredientByName(string name)
        {
            return FromEF(unitOfWork.IngredientsRepository.Get(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault());
        }

        public void UpdateIngredient(IngredientDto ing)
        {
            unitOfWork.IngredientsRepository.Update(FromPOCO(ing));
            unitOfWork.Save();
        }
    }
}
