namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_6_Attendance_Item_Stock_MaintCost_MaintReceipt_MaintReceiptDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendances",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Day = c.DateTime(nullable: false),
                        InTime = c.DateTime(nullable: false),
                        OutTime = c.DateTime(),
                        SecurityStaffId = c.Long(),
                        SocietyStaffId = c.Long(),
                        ShiftTypeId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SecurityStaffs", t => t.SecurityStaffId)
                .ForeignKey("dbo.MasterValues", t => t.ShiftTypeId)
                .ForeignKey("dbo.SocietyStaffs", t => t.SocietyStaffId)
                .Index(t => t.SecurityStaffId)
                .Index(t => t.SocietyStaffId)
                .Index(t => t.ShiftTypeId);
            
            CreateTable(
                "dbo.MaintenanceReceipts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Amount = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        ReceiptNo = c.Long(nullable: false),
                        AuthorizedBy = c.String(),
                        FlatId = c.Long(),
                        FlatOwnerId = c.Long(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Flats", t => t.FlatId)
                .ForeignKey("dbo.FlatOwners", t => t.FlatOwnerId)
                .Index(t => t.FlatId)
                .Index(t => t.FlatOwnerId);
            
            CreateTable(
                "dbo.MaintenanceReceiptDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Amount = c.Int(nullable: false),
                        MaintenanceReceiptId = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MaintenanceReceipts", t => t.MaintenanceReceiptId)
                .Index(t => t.MaintenanceReceiptId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        ItemTypeId = c.Int(nullable: false),
                        SocietyId = c.Long(nullable: false),
                        FacilityId = c.Long(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Facilities", t => t.FacilityId)
                .ForeignKey("dbo.MasterValues", t => t.ItemTypeId)
                .ForeignKey("dbo.Societies", t => t.SocietyId)
                .Index(t => t.ItemTypeId)
                .Index(t => t.SocietyId)
                .Index(t => t.FacilityId);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Unit = c.String(),
                        Type = c.Byte(nullable: false),
                        ItemId = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.MaintenanceCosts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Amount = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        FromDate = c.DateTime(),
                        ToDate = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.SecurityStaffs", "AadharCardNo", c => c.String());
            AddColumn("dbo.SecurityStaffs", "Photo", c => c.String());
            AddColumn("dbo.SecurityStaffs", "DateOfBirth", c => c.DateTime());
            AddColumn("dbo.SecurityStaffs", "Address", c => c.String());
            AddColumn("dbo.SecurityStaffs", "NativeAddress", c => c.String());
            AddColumn("dbo.SecurityStaffs", "JoiningDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SecurityStaffs", "LastWorkingDay", c => c.DateTime());
            AddColumn("dbo.SocietyStaffs", "AadharCardNo", c => c.String());
            AddColumn("dbo.SocietyStaffs", "Photo", c => c.String());
            AddColumn("dbo.SocietyStaffs", "DateOfBirth", c => c.DateTime());
            AddColumn("dbo.SocietyStaffs", "Address", c => c.String());
            AddColumn("dbo.SocietyStaffs", "NativeAddress", c => c.String());
            AddColumn("dbo.SocietyStaffs", "JoiningDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SocietyStaffs", "LastWorkingDay", c => c.DateTime());
            AddColumn("dbo.SocietyStaffs", "StaffTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.SocietyStaffs", "StaffTypeId");
            AddForeignKey("dbo.SocietyStaffs", "StaffTypeId", "dbo.MasterValues", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stocks", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Items", "SocietyId", "dbo.Societies");
            DropForeignKey("dbo.Items", "ItemTypeId", "dbo.MasterValues");
            DropForeignKey("dbo.Items", "FacilityId", "dbo.Facilities");
            DropForeignKey("dbo.MaintenanceReceipts", "FlatOwnerId", "dbo.FlatOwners");
            DropForeignKey("dbo.MaintenanceReceipts", "FlatId", "dbo.Flats");
            DropForeignKey("dbo.MaintenanceReceiptDetails", "MaintenanceReceiptId", "dbo.MaintenanceReceipts");
            DropForeignKey("dbo.SocietyStaffs", "StaffTypeId", "dbo.MasterValues");
            DropForeignKey("dbo.Attendances", "SocietyStaffId", "dbo.SocietyStaffs");
            DropForeignKey("dbo.Attendances", "ShiftTypeId", "dbo.MasterValues");
            DropForeignKey("dbo.Attendances", "SecurityStaffId", "dbo.SecurityStaffs");
            DropIndex("dbo.Stocks", new[] { "ItemId" });
            DropIndex("dbo.Items", new[] { "FacilityId" });
            DropIndex("dbo.Items", new[] { "SocietyId" });
            DropIndex("dbo.Items", new[] { "ItemTypeId" });
            DropIndex("dbo.MaintenanceReceiptDetails", new[] { "MaintenanceReceiptId" });
            DropIndex("dbo.MaintenanceReceipts", new[] { "FlatOwnerId" });
            DropIndex("dbo.MaintenanceReceipts", new[] { "FlatId" });
            DropIndex("dbo.SocietyStaffs", new[] { "StaffTypeId" });
            DropIndex("dbo.Attendances", new[] { "ShiftTypeId" });
            DropIndex("dbo.Attendances", new[] { "SocietyStaffId" });
            DropIndex("dbo.Attendances", new[] { "SecurityStaffId" });
            DropColumn("dbo.SocietyStaffs", "StaffTypeId");
            DropColumn("dbo.SocietyStaffs", "LastWorkingDay");
            DropColumn("dbo.SocietyStaffs", "JoiningDate");
            DropColumn("dbo.SocietyStaffs", "NativeAddress");
            DropColumn("dbo.SocietyStaffs", "Address");
            DropColumn("dbo.SocietyStaffs", "DateOfBirth");
            DropColumn("dbo.SocietyStaffs", "Photo");
            DropColumn("dbo.SocietyStaffs", "AadharCardNo");
            DropColumn("dbo.SecurityStaffs", "LastWorkingDay");
            DropColumn("dbo.SecurityStaffs", "JoiningDate");
            DropColumn("dbo.SecurityStaffs", "NativeAddress");
            DropColumn("dbo.SecurityStaffs", "Address");
            DropColumn("dbo.SecurityStaffs", "DateOfBirth");
            DropColumn("dbo.SecurityStaffs", "Photo");
            DropColumn("dbo.SecurityStaffs", "AadharCardNo");
            DropTable("dbo.MaintenanceCosts");
            DropTable("dbo.Stocks");
            DropTable("dbo.Items");
            DropTable("dbo.MaintenanceReceiptDetails");
            DropTable("dbo.MaintenanceReceipts");
            DropTable("dbo.Attendances");
        }
    }
}
