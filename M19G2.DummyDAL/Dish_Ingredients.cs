namespace M19G2.DummyDAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Dish_Ingredients
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public int dish_id { get; set; }

        public int ingredient_id { get; set; }

        public virtual Dish Dish { get; set; }

        public virtual Ingredient Ingredient { get; set; }
    }
}
