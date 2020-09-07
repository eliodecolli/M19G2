namespace M19G2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedUserRequestStatusToInt : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UsersAccessRequests", "AccessRequestStatus_Id", "dbo.AccessRequestStatus");
            DropIndex("dbo.UsersAccessRequests", new[] { "AccessRequestStatus_Id" });
            RenameColumn(table: "dbo.UsersAccessRequests", name: "AccessRequestStatus_Id", newName: "AccessRequestStatusId");
            AlterColumn("dbo.UsersAccessRequests", "AccessRequestStatusId", c => c.Int(nullable: false));
            CreateIndex("dbo.UsersAccessRequests", "AccessRequestStatusId");
            AddForeignKey("dbo.UsersAccessRequests", "AccessRequestStatusId", "dbo.AccessRequestStatus", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersAccessRequests", "AccessRequestStatusId", "dbo.AccessRequestStatus");
            DropIndex("dbo.UsersAccessRequests", new[] { "AccessRequestStatusId" });
            AlterColumn("dbo.UsersAccessRequests", "AccessRequestStatusId", c => c.Int());
            RenameColumn(table: "dbo.UsersAccessRequests", name: "AccessRequestStatusId", newName: "AccessRequestStatus_Id");
            CreateIndex("dbo.UsersAccessRequests", "AccessRequestStatus_Id");
            AddForeignKey("dbo.UsersAccessRequests", "AccessRequestStatus_Id", "dbo.AccessRequestStatus", "Id");
        }
    }
}
