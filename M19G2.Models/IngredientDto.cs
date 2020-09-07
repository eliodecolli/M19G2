using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace M19G2.Models
{
    public class IngredientDto
    {
        [Display(Name = "Number")]
        public int ID { get; set; }
        
        //[Required]
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}
