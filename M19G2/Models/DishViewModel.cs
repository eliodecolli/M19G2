using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace M19G2.Models
{
    public class DishViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DateTime createdOn { get; set; }
        public IEnumerable<IngredientDto> Ingredients { get; set; }

        //[Display(Name = "Ingredients")]
        //public byte IngredientsId { get; set; }
    }
}