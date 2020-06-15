namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Main_Cost_SocietyId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MaintenanceCosts", "SocietyId", c => c.Long(nullable: false));
            CreateIndex("dbo.MaintenanceCosts", "SocietyId");
            AddForeignKey("dbo.MaintenanceCosts", "SocietyId", "dbo.Societies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MaintenanceCosts", "SocietyId", "dbo.Societies");
            DropIndex("dbo.MaintenanceCosts", new[] { "SocietyId" });
            DropColumn("dbo.MaintenanceCosts", "SocietyId");
        }
    }
}
