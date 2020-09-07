namespace M19G2.DummyDAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            Order_Dishes = new HashSet<Order_Dishes>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [Required]
        [StringLength(128)]
        public string user_id { get; set; }

        public bool is_cancelled { get; set; }

        [Required]
        public string message { get; set; }

        public bool is_delivered { get; set; }

        public bool is_completed { get; set; }

        public int address_id { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Dishes> Order_Dishes { get; set; }

        public virtual User_Adresses User_Adresses { get; set; }
    }
}
