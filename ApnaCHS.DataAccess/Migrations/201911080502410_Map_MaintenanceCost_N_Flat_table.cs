namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Map_MaintenanceCost_N_Flat_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MapMaintenanceCostsToFlats",
                c => new
                    {
                        MaintenanceCostId = c.Long(nullable: false),
                        FlatId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.MaintenanceCostId, t.FlatId })
                .ForeignKey("dbo.MaintenanceCosts", t => t.MaintenanceCostId, cascadeDelete: true)
                .ForeignKey("dbo.Flats", t => t.FlatId, cascadeDelete: true)
                .Index(t => t.MaintenanceCostId)
                .Index(t => t.FlatId);
            
            AddColumn("dbo.MaintenanceCosts", "Name", c => c.String());
            AddColumn("dbo.MaintenanceCosts", "CalculationOnArea", c => c.Boolean(nullable: false));
            AddColumn("dbo.MaintenanceCosts", "AllFlats", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MapMaintenanceCostsToFlats", "FlatId", "dbo.Flats");
            DropForeignKey("dbo.MapMaintenanceCostsToFlats", "MaintenanceCostId", "dbo.MaintenanceCosts");
            DropIndex("dbo.MapMaintenanceCostsToFlats", new[] { "FlatId" });
            DropIndex("dbo.MapMaintenanceCostsToFlats", new[] { "MaintenanceCostId" });
            DropColumn("dbo.MaintenanceCosts", "AllFlats");
            DropColumn("dbo.MaintenanceCosts", "CalculationOnArea");
            DropColumn("dbo.MaintenanceCosts", "Name");
            DropTable("dbo.MapMaintenanceCostsToFlats");
        }
    }
}
