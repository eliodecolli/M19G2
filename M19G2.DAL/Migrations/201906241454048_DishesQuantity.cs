namespace M19G2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DishesQuantity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderQuantities",
                c => new
                    {
                        OrderID = c.Int(nullable: false),
                        DishID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrderID, t.DishID });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OrderQuantities");
        }
    }
}
