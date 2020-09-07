namespace M19G2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserAccessRequestsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessRequestStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UsersAccessRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Gender = c.String(maxLength: 1),
                        Birthday = c.DateTime(),
                        PhoneNumber = c.String(nullable: false, maxLength: 256),
                        AspNetRoleId = c.Int(nullable: false),
                        AccessRequestStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccessRequestStatus", t => t.AccessRequestStatus_Id)
                .ForeignKey("dbo.AspNetRoles", t => t.AspNetRoleId, cascadeDelete: true)
                .Index(t => t.AspNetRoleId)
                .Index(t => t.AccessRequestStatus_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersAccessRequests", "AspNetRoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.UsersAccessRequests", "AccessRequestStatus_Id", "dbo.AccessRequestStatus");
            DropIndex("dbo.UsersAccessRequests", new[] { "AccessRequestStatus_Id" });
            DropIndex("dbo.UsersAccessRequests", new[] { "AspNetRoleId" });
            DropTable("dbo.UsersAccessRequests");
            DropTable("dbo.AccessRequestStatus");
        }
    }
}
