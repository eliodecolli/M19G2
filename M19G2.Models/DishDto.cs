using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace M19G2.Models
{
    public class DishDto
    {
        public int DishID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } 

        /// <summary>
        /// The ID of the Dish_Type entity related to this instance.
        /// </summary>
        public int DishType { get; set; }

        public string DishTypeName { get; set; }
        
        public List<IngredientDto> Ingredients { get; set; }

        public List<int> ImagesId { get; set; }
    }
}
