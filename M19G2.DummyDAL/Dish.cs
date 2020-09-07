namespace M19G2.DummyDAL
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
            Dish_Ingredients = new HashSet<Dish_Ingredients>();
            Order_Dishes = new HashSet<Order_Dishes>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [StringLength(250)]
        public string description { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        public DateTime created_on { get; set; }

        public decimal price { get; set; }

        public bool is_active { get; set; }

        public int dish_type_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dish_Ingredients> Dish_Ingredients { get; set; }

        public virtual Dish_Type Dish_Type { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Dishes> Order_Dishes { get; set; }
    }
}
