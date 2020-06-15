namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_3_table_SocietyDocument_Vehicle_FlatParking : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FlatParkings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        FlatId = c.Long(nullable: false),
                        FacilityId = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Facilities", t => t.FacilityId)
                .ForeignKey("dbo.Flats", t => t.FlatId)
                .Index(t => t.FlatId)
                .Index(t => t.FacilityId);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Make = c.String(),
                        Number = c.String(),
                        FlatId = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Flats", t => t.FlatId)
                .Index(t => t.FlatId);
            
            CreateTable(
                "dbo.SocietyDocuments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        FilePath = c.String(),
                        SocietyId = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Societies", t => t.SocietyId)
                .Index(t => t.SocietyId);
            
            AddColumn("dbo.Facilities", "WithLift", c => c.Boolean());
            AddColumn("dbo.Facilities", "TotalParkingSlots", c => c.Int());
            AddColumn("dbo.Facilities", "TotalNoFlats", c => c.Int());
            AddColumn("dbo.Facilities", "FacilityTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.Floors", "TotalNoFlats", c => c.Int(nullable: false));
            AddColumn("dbo.Floors", "FloorTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.Flats", "TotalArea", c => c.String());
            AddColumn("dbo.Flats", "CarpetArea", c => c.String());
            AddColumn("dbo.Flats", "Bathroom", c => c.Int());
            AddColumn("dbo.Flats", "HaveParking", c => c.Boolean(nullable: false));
            AddColumn("dbo.Flats", "IsRented", c => c.Boolean(nullable: false));
            AddColumn("dbo.FlatOwners", "MobileNo", c => c.String());
            AddColumn("dbo.FlatOwners", "EmailId", c => c.String());
            AddColumn("dbo.FlatOwners", "DateOfBirth", c => c.DateTime());
            AddColumn("dbo.FlatOwners", "MemberSinceDate", c => c.DateTime());
            AddColumn("dbo.FlatOwnerFamilies", "AdminMember", c => c.Boolean(nullable: false));
            AddColumn("dbo.FlatOwnerFamilies", "ApproverMember", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tenants", "IsPayGuest", c => c.Boolean(nullable: false));
            AddColumn("dbo.Societies", "Address", c => c.String());
            AddColumn("dbo.Societies", "Area", c => c.String());
            AddColumn("dbo.Societies", "RegistrationNo", c => c.String());
            AddColumn("dbo.Societies", "DateOfIncorporation", c => c.DateTime());
            AddColumn("dbo.Societies", "DateOfRegistration", c => c.DateTime());
            AddColumn("dbo.Societies", "NoOfBuilding", c => c.Int());
            AddColumn("dbo.Societies", "WithLift", c => c.Boolean());
            AddColumn("dbo.Societies", "NoOfGate", c => c.Int());
            AddColumn("dbo.Societies", "TotalParkingSlots", c => c.Int());
            AddColumn("dbo.Societies", "TotalNoFlats", c => c.Int());
            AddColumn("dbo.Societies", "SharedComplex", c => c.Boolean());
            AddColumn("dbo.Societies", "SharedResources", c => c.Boolean());
            CreateIndex("dbo.Facilities", "FacilityTypeId");
            CreateIndex("dbo.Floors", "FloorTypeId");
            AddForeignKey("dbo.Facilities", "FacilityTypeId", "dbo.MasterValues", "Id");
            AddForeignKey("dbo.Floors", "FloorTypeId", "dbo.MasterValues", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SocietyDocuments", "SocietyId", "dbo.Societies");
            DropForeignKey("dbo.Vehicles", "FlatId", "dbo.Flats");
            DropForeignKey("dbo.Floors", "FloorTypeId", "dbo.MasterValues");
            DropForeignKey("dbo.FlatParkings", "FlatId", "dbo.Flats");
            DropForeignKey("dbo.FlatParkings", "FacilityId", "dbo.Facilities");
            DropForeignKey("dbo.Facilities", "FacilityTypeId", "dbo.MasterValues");
            DropIndex("dbo.SocietyDocuments", new[] { "SocietyId" });
            DropIndex("dbo.Vehicles", new[] { "FlatId" });
            DropIndex("dbo.Floors", new[] { "FloorTypeId" });
            DropIndex("dbo.FlatParkings", new[] { "FacilityId" });
            DropIndex("dbo.FlatParkings", new[] { "FlatId" });
            DropIndex("dbo.Facilities", new[] { "FacilityTypeId" });
            DropColumn("dbo.Societies", "SharedResources");
            DropColumn("dbo.Societies", "SharedComplex");
            DropColumn("dbo.Societies", "TotalNoFlats");
            DropColumn("dbo.Societies", "TotalParkingSlots");
            DropColumn("dbo.Societies", "NoOfGate");
            DropColumn("dbo.Societies", "WithLift");
            DropColumn("dbo.Societies", "NoOfBuilding");
            DropColumn("dbo.Societies", "DateOfRegistration");
            DropColumn("dbo.Societies", "DateOfIncorporation");
            DropColumn("dbo.Societies", "RegistrationNo");
            DropColumn("dbo.Societies", "Area");
            DropColumn("dbo.Societies", "Address");
            DropColumn("dbo.Tenants", "IsPayGuest");
            DropColumn("dbo.FlatOwnerFamilies", "ApproverMember");
            DropColumn("dbo.FlatOwnerFamilies", "AdminMember");
            DropColumn("dbo.FlatOwners", "MemberSinceDate");
            DropColumn("dbo.FlatOwners", "DateOfBirth");
            DropColumn("dbo.FlatOwners", "EmailId");
            DropColumn("dbo.FlatOwners", "MobileNo");
            DropColumn("dbo.Flats", "IsRented");
            DropColumn("dbo.Flats", "HaveParking");
            DropColumn("dbo.Flats", "Bathroom");
            DropColumn("dbo.Flats", "CarpetArea");
            DropColumn("dbo.Flats", "TotalArea");
            DropColumn("dbo.Floors", "FloorTypeId");
            DropColumn("dbo.Floors", "TotalNoFlats");
            DropColumn("dbo.Facilities", "FacilityTypeId");
            DropColumn("dbo.Facilities", "TotalNoFlats");
            DropColumn("dbo.Facilities", "TotalParkingSlots");
            DropColumn("dbo.Facilities", "WithLift");
            DropTable("dbo.SocietyDocuments");
            DropTable("dbo.Vehicles");
            DropTable("dbo.FlatParkings");
        }
    }
}
