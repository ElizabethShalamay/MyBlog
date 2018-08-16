namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Approvements : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "IsApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Posts", "IsApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "IsApproved");
            DropColumn("dbo.Comments", "IsApproved");
        }
    }
}
