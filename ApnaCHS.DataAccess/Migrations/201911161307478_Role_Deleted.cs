namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Role_Deleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Roles", "Deleted", c => c.Boolean(nullable: false));
            DropColumn("dbo.Roles", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Roles", "IsActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.Roles", "Deleted");
        }
    }
}
