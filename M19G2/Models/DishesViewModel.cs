using System.Collections.Generic;

namespace M19G2.Models
{
    public class DishesViewModel
    {
        public List<DishDto> Dishes { get; set; }
        public List<DishTypeDto> Types { get; set; } 
        //public bool IsFiltered { get; set; }
    }
}