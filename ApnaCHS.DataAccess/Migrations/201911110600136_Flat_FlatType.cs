namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Flat_FlatType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Flats", "FlatTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Flats", "FlatTypeId");
            AddForeignKey("dbo.Flats", "FlatTypeId", "dbo.MasterValues", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Flats", "FlatTypeId", "dbo.MasterValues");
            DropIndex("dbo.Flats", new[] { "FlatTypeId" });
            DropColumn("dbo.Flats", "FlatTypeId");
        }
    }
}
