namespace M19G2.DummyDAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=dummyDal")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Dish_Ingredients> Dish_Ingredients { get; set; }
        public virtual DbSet<Dish_Type> Dish_Type { get; set; }
        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<Order_Dishes> Order_Dishes { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User_Adresses> User_Adresses { get; set; }

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
                .HasForeignKey(e => e.user_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.User_Adresses)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.user_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dish_Type>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Dish_Type>()
                .HasMany(e => e.Dishes)
                .WithRequired(e => e.Dish_Type)
                .HasForeignKey(e => e.dish_type_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dish>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Dish>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Dish>()
                .Property(e => e.price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Dish>()
                .HasMany(e => e.Dish_Ingredients)
                .WithRequired(e => e.Dish)
                .HasForeignKey(e => e.dish_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dish>()
                .HasMany(e => e.Order_Dishes)
                .WithRequired(e => e.Dish)
                .HasForeignKey(e => e.dish_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ingredient>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Ingredient>()
                .HasMany(e => e.Dish_Ingredients)
                .WithRequired(e => e.Ingredient)
                .HasForeignKey(e => e.ingredient_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.message)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.Order_Dishes)
                .WithRequired(e => e.Order)
                .HasForeignKey(e => e.order_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User_Adresses>()
                .Property(e => e.street_name)
                .IsUnicode(false);

            modelBuilder.Entity<User_Adresses>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.User_Adresses)
                .HasForeignKey(e => e.address_id)
                .WillCascadeOnDelete(false);
        }
    }
}
