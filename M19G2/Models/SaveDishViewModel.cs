using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using M19G2.DAL.Entities;

namespace M19G2.Models
{
    public class SaveDishViewModel
    {
        
        public DishDto DishDto { get; set; }
        public List<IngredientDto> Ingredients { get; set; }
        public List<DishTypeDto> DishTypeDtos { get; set; }
    }
}