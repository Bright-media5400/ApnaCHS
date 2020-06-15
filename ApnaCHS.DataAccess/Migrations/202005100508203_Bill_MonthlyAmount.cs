namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bill_MonthlyAmount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "MonthlyAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bills", "MonthlyAmount");
        }
    }
}
