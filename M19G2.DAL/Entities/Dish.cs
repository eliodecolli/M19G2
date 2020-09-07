namespace M19G2.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Dish
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Dish()
        {
            //Dish_Ingredients = new HashSet<Dish_Ingredients>();
           // Order_Dishes = new HashSet<Order_Dishes>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public int TypeID { get; set; }
        
        public virtual ICollection<Ingredient> Ingredients { get; set; }

        public virtual DishType Type { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Image> Images { get; set; }
    }
}
