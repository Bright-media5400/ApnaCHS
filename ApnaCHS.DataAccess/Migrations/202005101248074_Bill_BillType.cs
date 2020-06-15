namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bill_BillType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "BillType", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bills", "BillType");
        }
    }
}
