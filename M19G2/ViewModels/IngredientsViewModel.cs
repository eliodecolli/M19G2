using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using M19G2.Models;

namespace M19G2.ViewModels
{
    public class IngredientsViewModel
    {
        public IEnumerable<IngredientDto> IngredientDtos { get; set; }
        public IngredientDto IngredientDto { get; set; }
    }
}