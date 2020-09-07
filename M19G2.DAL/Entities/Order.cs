namespace M19G2.DAL.Entities
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
           // Order_Dishes = new HashSet<Order_Dishes>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public int UserID { get; set; }

        public bool IsCancelled { get; set; }

        public string Message { get; set; }

        public bool IsDelivered { get; set; }

        public bool IsCompleted { get; set; }

        public int AddressID { get; set; }

        public int? ETA { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dish> Dishes { get; set; }

        public virtual UserAddress UserAddress { get; set; }

        public virtual DateTime DateCreated { get; set; }
    }
}
