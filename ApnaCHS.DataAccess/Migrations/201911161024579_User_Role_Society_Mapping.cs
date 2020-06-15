namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_Role_Society_Mapping : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Societies", "Email", c => c.String());
            AddColumn("dbo.Societies", "PhoneNo", c => c.String());
            AddColumn("dbo.Societies", "ContactPerson", c => c.String());
            AddColumn("dbo.Roles", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Roles", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Roles", "CreatedBy", c => c.String());
            AddColumn("dbo.Roles", "ModifiedBy", c => c.String());
            AddColumn("dbo.Roles", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Roles", "IsDefault", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Roles", "IsDefault");
            DropColumn("dbo.Roles", "IsActive");
            DropColumn("dbo.Roles", "ModifiedBy");
            DropColumn("dbo.Roles", "CreatedBy");
            DropColumn("dbo.Roles", "ModifiedDate");
            DropColumn("dbo.Roles", "CreatedDate");
            DropColumn("dbo.Societies", "ContactPerson");
            DropColumn("dbo.Societies", "PhoneNo");
            DropColumn("dbo.Societies", "Email");
        }
    }
}
