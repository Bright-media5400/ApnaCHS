namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParkingLot_Added : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.FlatParkings", new[] { "FlatId" });
            DropIndex("dbo.FlatParkings", new[] { "FacilityId" });
            RenameColumn(table: "dbo.FlatParkings", name: "FlatId", newName: "Flat_Id");
            AddColumn("dbo.Facilities", "IsParkingLot", c => c.Boolean(nullable: false));
            AddColumn("dbo.FlatParkings", "SqftArea", c => c.Int(nullable: false));
            AddColumn("dbo.FlatParkings", "ParkingTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.FlatParkings", "FloorId", c => c.Long());
            AddColumn("dbo.Floors", "IsParkingLot", c => c.Boolean(nullable: false));
            AlterColumn("dbo.FlatParkings", "Flat_Id", c => c.Long());
            AlterColumn("dbo.FlatParkings", "FacilityId", c => c.Long());
            CreateIndex("dbo.FlatParkings", "ParkingTypeId");
            CreateIndex("dbo.FlatParkings", "FacilityId");
            CreateIndex("dbo.FlatParkings", "FloorId");
            CreateIndex("dbo.FlatParkings", "Flat_Id");
            AddForeignKey("dbo.FlatParkings", "FloorId", "dbo.Floors", "Id");
            AddForeignKey("dbo.FlatParkings", "ParkingTypeId", "dbo.MasterValues", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FlatParkings", "ParkingTypeId", "dbo.MasterValues");
            DropForeignKey("dbo.FlatParkings", "FloorId", "dbo.Floors");
            DropIndex("dbo.FlatParkings", new[] { "Flat_Id" });
            DropIndex("dbo.FlatParkings", new[] { "FloorId" });
            DropIndex("dbo.FlatParkings", new[] { "FacilityId" });
            DropIndex("dbo.FlatParkings", new[] { "ParkingTypeId" });
            AlterColumn("dbo.FlatParkings", "FacilityId", c => c.Long(nullable: false));
            AlterColumn("dbo.FlatParkings", "Flat_Id", c => c.Long(nullable: false));
            DropColumn("dbo.Floors", "IsParkingLot");
            DropColumn("dbo.FlatParkings", "FloorId");
            DropColumn("dbo.FlatParkings", "ParkingTypeId");
            DropColumn("dbo.FlatParkings", "SqftArea");
            DropColumn("dbo.Facilities", "IsParkingLot");
            RenameColumn(table: "dbo.FlatParkings", name: "Flat_Id", newName: "FlatId");
            CreateIndex("dbo.FlatParkings", "FacilityId");
            CreateIndex("dbo.FlatParkings", "FlatId");
        }
    }
}
