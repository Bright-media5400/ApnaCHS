namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Map_Flat_N_FlatOwner_table : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FlatOwners", "FlatId", "dbo.Flats");
            DropIndex("dbo.FlatOwners", new[] { "FlatId" });
            CreateTable(
                "dbo.MapFlatsToFlatOwners",
                c => new
                    {
                        FlatId = c.Long(nullable: false),
                        FlatOwnerId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.FlatId, t.FlatOwnerId })
                .ForeignKey("dbo.Flats", t => t.FlatId, cascadeDelete: true)
                .ForeignKey("dbo.FlatOwners", t => t.FlatOwnerId, cascadeDelete: true)
                .Index(t => t.FlatId)
                .Index(t => t.FlatOwnerId);
            
            DropColumn("dbo.FlatOwners", "FlatId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FlatOwners", "FlatId", c => c.Long(nullable: false));
            DropForeignKey("dbo.MapFlatsToFlatOwners", "FlatOwnerId", "dbo.FlatOwners");
            DropForeignKey("dbo.MapFlatsToFlatOwners", "FlatId", "dbo.Flats");
            DropIndex("dbo.MapFlatsToFlatOwners", new[] { "FlatOwnerId" });
            DropIndex("dbo.MapFlatsToFlatOwners", new[] { "FlatId" });
            DropTable("dbo.MapFlatsToFlatOwners");
            CreateIndex("dbo.FlatOwners", "FlatId");
            AddForeignKey("dbo.FlatOwners", "FlatId", "dbo.Flats", "Id");
        }
    }
}
