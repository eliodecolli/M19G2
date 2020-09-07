using M19G2.DAL.Entities;

namespace M19G2.DAL.Persistence
{
    using System.Data.Entity;

    //removed partial am here cuz at this point is unnecessary
    public class M19G2Context : DbContext
    {
        public M19G2Context()
            : base("name=M19G2Context")
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        //public virtual DbSet<Dish_Ingredients> Dish_Ingredients { get; set; }
        public virtual DbSet<DishType> Dish_Type { get; set; }
        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }

        public virtual DbSet<Log4NetLog> Log4NetLog { get; set; }
        //public virtual DbSet<Order_Dishes> Order_Dishes { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<UserAddress> User_Adresses { get; set; }

        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<UsersAccessRequest> UsersAccessRequests { get; set; }
        public virtual DbSet<AccessRequestStatus> AccessRequestStatuses { get; set; }
        public virtual DbSet<OrderQuantity> OrdersCapacities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.UserAddresses)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Log4NetLog>()
                .Property(e => e.Thread)
                .IsUnicode(false);

            modelBuilder.Entity<Log4NetLog>()
                .Property(e => e.Level)
                .IsUnicode(false);

            modelBuilder.Entity<Log4NetLog>()
                .Property(e => e.Logger)
                .IsUnicode(false);

            modelBuilder.Entity<Log4NetLog>()
                .Property(e => e.Message)
                .IsUnicode(false);

            modelBuilder.Entity<Log4NetLog>()
                .Property(e => e.Exception)
                .IsUnicode(false);

            modelBuilder.Entity<DishType>()
                .HasKey(dt => dt.ID);

            modelBuilder.Entity<DishType>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<DishType>()
                .HasMany(e => e.Dishes)
                .WithRequired(e => e.Type)
                .HasForeignKey(e => e.TypeID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dish>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Dish>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Dish>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Dish>()
                .HasMany(d => d.Ingredients)
                .WithMany(i => i.Dishes);

            modelBuilder.Entity<Dish>()
                .HasMany(d => d.Images)
                .WithRequired(i => i.Dish)
                .HasForeignKey(i => i.DishID);

            modelBuilder.Entity<Ingredient>()
                .HasKey(i => i.ID);
            modelBuilder.Entity<Ingredient>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .HasRequired(o => o.AspNetUser)
                .WithMany(x => x.Orders)
                .HasForeignKey(o => o.UserID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasRequired(o => o.UserAddress)
                .WithMany(a => a.Orders)
                .HasForeignKey(o => o.AddressID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Dishes)
                .WithMany(d => d.Orders);

            modelBuilder.Entity<UserAddress>()
                .Property(e => e.StreetName)
                .IsUnicode(false);

            modelBuilder.Entity<OrderQuantity>()
                .HasKey(k => new { k.OrderID, k.DishID });

            /*modelBuilder.Entity<UserAddress>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.UserAddress)
                .HasForeignKey(e => e.AddressID)
                .WillCascadeOnDelete(false);*/
        }
    }
}
