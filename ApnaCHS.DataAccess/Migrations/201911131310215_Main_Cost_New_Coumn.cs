namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Main_Cost_New_Coumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MaintenanceCosts", "PerSqrArea", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.MaintenanceCosts", "IsApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.MaintenanceCosts", "ApprovedDate", c => c.DateTime());
            AddColumn("dbo.MaintenanceCosts", "ApprovedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MaintenanceCosts", "ApprovedBy");
            DropColumn("dbo.MaintenanceCosts", "ApprovedDate");
            DropColumn("dbo.MaintenanceCosts", "IsApproved");
            DropColumn("dbo.MaintenanceCosts", "PerSqrArea");
        }
    }
}
