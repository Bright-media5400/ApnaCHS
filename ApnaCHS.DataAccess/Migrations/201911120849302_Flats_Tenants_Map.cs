namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Flats_Tenants_Map : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tenants", "FlatId", "dbo.Flats");
            DropIndex("dbo.Tenants", new[] { "FlatId" });
            CreateTable(
                "dbo.MapFlatsToTenants",
                c => new
                    {
                        TenantId = c.Long(nullable: false),
                        FlatId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.TenantId, t.FlatId })
                .ForeignKey("dbo.Tenants", t => t.TenantId, cascadeDelete: true)
                .ForeignKey("dbo.Flats", t => t.FlatId, cascadeDelete: true)
                .Index(t => t.TenantId)
                .Index(t => t.FlatId);
            
            DropColumn("dbo.Tenants", "FlatId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tenants", "FlatId", c => c.Long(nullable: false));
            DropForeignKey("dbo.MapFlatsToTenants", "FlatId", "dbo.Flats");
            DropForeignKey("dbo.MapFlatsToTenants", "TenantId", "dbo.Tenants");
            DropIndex("dbo.MapFlatsToTenants", new[] { "FlatId" });
            DropIndex("dbo.MapFlatsToTenants", new[] { "TenantId" });
            DropTable("dbo.MapFlatsToTenants");
            CreateIndex("dbo.Tenants", "FlatId");
            AddForeignKey("dbo.Tenants", "FlatId", "dbo.Flats", "Id");
        }
    }
}
