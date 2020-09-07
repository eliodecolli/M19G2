namespace M19G2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedStatusIdToAccessRequestStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccessRequestStatus", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccessRequestStatus", "Status");
        }
    }
}
