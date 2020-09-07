using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace M19G2.Models
{
    public class DishFilterViewModel
    {
        [Display(Name = "Filter")]
        public bool FilterActive => !string.IsNullOrEmpty(DishFilterName) || !string.IsNullOrEmpty(DishFilterTypeName);

        [Display(Name = "Dish Name")]
        public string DishFilterName { get; set; }

        [Display(Name = "Dish Type")]
        public string DishFilterTypeName { get; set; }

        public List<SelectListItem> DishTypes { get; set; }
    }
}