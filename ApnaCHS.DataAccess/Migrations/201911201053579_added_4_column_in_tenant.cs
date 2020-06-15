namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_4_column_in_tenant : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tenants", "MobileNo", c => c.String());
            AddColumn("dbo.Tenants", "EmailId", c => c.String());
            AddColumn("dbo.Tenants", "DateOfBirth", c => c.DateTime());
            AddColumn("dbo.Tenants", "MemberSinceDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tenants", "MemberSinceDate");
            DropColumn("dbo.Tenants", "DateOfBirth");
            DropColumn("dbo.Tenants", "EmailId");
            DropColumn("dbo.Tenants", "MobileNo");
        }
    }
}
