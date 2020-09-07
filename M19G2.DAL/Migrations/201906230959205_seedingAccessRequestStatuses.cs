namespace M19G2.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seedingAccessRequestStatuses : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO AccessRequestStatus (Status, Description) VALUES (0, 'Pending')");

            Sql("INSERT INTO AccessRequestStatus (Status, Description) VALUES (1, 'Approved')");

            Sql("INSERT INTO AccessRequestStatus (Status, Description) VALUES (2, 'Denied')");
        }
        
        public override void Down()
        {
        }
    }
}
