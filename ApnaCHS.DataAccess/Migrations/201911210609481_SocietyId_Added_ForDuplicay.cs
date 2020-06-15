namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SocietyId_Added_ForDuplicay : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Flats", "SocietyId", c => c.Long(nullable: false));
            AddColumn("dbo.Floors", "SocietyId", c => c.Long(nullable: false));
            AddColumn("dbo.Roles", "DisplayName", c => c.String());
            CreateIndex("dbo.Flats", "SocietyId");
            CreateIndex("dbo.Floors", "SocietyId");
            AddForeignKey("dbo.Floors", "SocietyId", "dbo.Societies", "Id");
            AddForeignKey("dbo.Flats", "SocietyId", "dbo.Societies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Flats", "SocietyId", "dbo.Societies");
            DropForeignKey("dbo.Floors", "SocietyId", "dbo.Societies");
            DropIndex("dbo.Floors", new[] { "SocietyId" });
            DropIndex("dbo.Flats", new[] { "SocietyId" });
            DropColumn("dbo.Roles", "DisplayName");
            DropColumn("dbo.Floors", "SocietyId");
            DropColumn("dbo.Flats", "SocietyId");
        }
    }
}
