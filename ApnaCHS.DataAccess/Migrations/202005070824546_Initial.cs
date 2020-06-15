namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AllIndiaPincodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OfficeName = c.String(),
                        Pincode = c.Int(nullable: false),
                        OfficeType = c.String(),
                        DeliveryStatus = c.String(),
                        DivisionName = c.String(),
                        RegionName = c.String(),
                        CircleName = c.String(),
                        Taluk = c.String(),
                        DistrictName = c.String(),
                        StateName = c.String(),
                        Telephone = c.String(),
                        RelatedSuboffice = c.String(),
                        RelatedHeadoffice = c.String(),
                        Longitude = c.String(),
                        Latitude = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SecurityStaffs", t => t.SecurityStaffId)
                .ForeignKey("dbo.SocietyStaffs", t => t.SocietyStaffId)
                .ForeignKey("dbo.MasterValues", t => t.ShiftTypeId)
                .Index(t => t.SecurityStaffId)
                .Index(t => t.SocietyStaffId)
                .Index(t => t.ShiftTypeId);
            
            CreateTable(
                "dbo.SecurityStaffs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        PhoneNo = c.String(),
                        AadharCardNo = c.String(),
                        Photo = c.String(),
                        DateOfBirth = c.DateTime(),
                        Address = c.String(),
                        NativeAddress = c.String(),
                        JoiningDate = c.DateTime(nullable: false),
                        LastWorkingDay = c.DateTime(),
                        SocietyId = c.Long(nullable: false),
                        ShiftTypeId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MasterValues", t => t.ShiftTypeId)
                .ForeignKey("dbo.Societies", t => t.SocietyId)
                .Index(t => t.SocietyId)
                .Index(t => t.ShiftTypeId);
            
            CreateTable(
                "dbo.MasterValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Description = c.String(),
                        Type = c.Byte(nullable: false),
                        CustomFields = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Societies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        RegistrationNo = c.String(),
                        DateOfIncorporation = c.DateTime(),
                        DateOfRegistration = c.DateTime(),
                        Email = c.String(),
                        PhoneNo = c.String(),
                        ContactPerson = c.String(),
                        BillingCycle = c.Int(nullable: false),
                        DueDays = c.Int(nullable: false),
                        Second2Wheeler = c.Long(),
                        Second4Wheeler = c.Long(),
                        InterestPercent = c.Decimal(precision: 18, scale: 2),
                        ApprovalsCount = c.Int(nullable: false),
                        ComplexId = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Complexes", t => t.ComplexId, cascadeDelete: true)
                .Index(t => t.ComplexId);
            
            CreateTable(
                "dbo.Complexes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Type = c.Byte(nullable: false),
                        Name = c.String(),
                        Address = c.String(),
                        Area = c.String(),
                        CityId = c.Int(nullable: false),
                        StateId = c.Int(nullable: false),
                        RegistrationNo = c.String(),
                        DateOfIncorporation = c.DateTime(),
                        DateOfRegistration = c.DateTime(),
                        NoOfBuilding = c.Int(nullable: false),
                        NoOfGate = c.Int(nullable: false),
                        NoOfSocieties = c.Int(nullable: false),
                        NoOfAmenities = c.Int(nullable: false),
                        Email = c.String(),
                        PhoneNo = c.String(),
                        ContactPerson = c.String(),
                        Pincode = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MasterValues", t => t.CityId)
                .ForeignKey("dbo.MasterValues", t => t.StateId)
                .Index(t => t.CityId)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.Facilities",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Wing = c.String(),
                        NoOfFloors = c.Int(nullable: false),
                        NoOfFlats = c.Int(nullable: false),
                        NoOfLifts = c.Int(nullable: false),
                        NoOfParkinglots = c.Int(nullable: false),
                        IsParkingLot = c.Boolean(nullable: false),
                        ComplexId = c.Long(nullable: false),
                        Type = c.Byte(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Complexes", t => t.ComplexId, cascadeDelete: true)
                .Index(t => t.ComplexId);
            
            CreateTable(
                "dbo.FlatParkings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        ParkingNo = c.Int(nullable: false),
                        SqftArea = c.Int(nullable: false),
                        Type = c.Byte(),
                        FacilityId = c.Long(),
                        FloorId = c.Long(),
                        FlatId = c.Long(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Facilities", t => t.FacilityId)
                .ForeignKey("dbo.Flats", t => t.FlatId)
                .ForeignKey("dbo.Floors", t => t.FloorId)
                .Index(t => t.FacilityId)
                .Index(t => t.FloorId)
                .Index(t => t.FlatId);
            
            CreateTable(
                "dbo.Flats",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        TotalArea = c.Int(nullable: false),
                        CarpetArea = c.Int(nullable: false),
                        BuildUpArea = c.Int(),
                        HaveParking = c.Boolean(nullable: false),
                        IsRented = c.Boolean(nullable: false),
                        IsCommercialSpace = c.Boolean(nullable: false),
                        FloorId = c.Long(nullable: false),
                        FlatTypeId = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MasterValues", t => t.FlatTypeId)
                .ForeignKey("dbo.Floors", t => t.FloorId)
                .Index(t => t.FloorId)
                .Index(t => t.FlatTypeId);
            
            CreateTable(
                "dbo.DataApprovals",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FlatId = c.Long(),
                        FlatOwnerId = c.Long(),
                        MaintenanceCostId = c.Long(),
                        FlatOwnerFamilyId = c.Long(),
                        VehicleId = c.Long(),
                        ApprovedDate = c.DateTime(),
                        ApprovedBy = c.String(),
                        ApprovedName = c.String(),
                        Note = c.String(),
                        ApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Flats", t => t.FlatId)
                .ForeignKey("dbo.FlatOwners", t => t.FlatOwnerId)
                .ForeignKey("dbo.FlatOwnerFamilies", t => t.FlatOwnerFamilyId)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId)
                .ForeignKey("dbo.MaintenanceCosts", t => t.MaintenanceCostId, cascadeDelete: true)
                .Index(t => t.FlatId)
                .Index(t => t.FlatOwnerId)
                .Index(t => t.MaintenanceCostId)
                .Index(t => t.FlatOwnerFamilyId)
                .Index(t => t.VehicleId);
            
            CreateTable(
                "dbo.FlatOwners",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        MobileNo = c.String(),
                        EmailId = c.String(),
                        DateOfBirth = c.DateTime(),
                        AadhaarCardNo = c.String(),
                        GenderId = c.Int(nullable: false),
                        UserId = c.Long(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.MasterValues", t => t.GenderId)
                .Index(t => t.GenderId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        Name = c.String(),
                        MaxAttempts = c.Int(nullable: false),
                        bBlocked = c.Boolean(nullable: false),
                        bChangePass = c.Boolean(nullable: false),
                        IsBack = c.Boolean(nullable: false),
                        IsDefault = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.MapUserToComplexes",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        ComplexId = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ComplexId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Complexes", t => t.ComplexId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ComplexId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DisplayName = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsBack = c.Boolean(),
                        IsDefault = c.Boolean(nullable: false),
                        SocietyId = c.Long(),
                        ComplexId = c.Long(),
                        CanChange = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Complexes", t => t.ComplexId)
                .ForeignKey("dbo.Societies", t => t.SocietyId)
                .Index(t => t.SocietyId)
                .Index(t => t.ComplexId)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.MapUserToSocieties",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        SocietyId = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                        IsBlocked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.SocietyId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Societies", t => t.SocietyId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.SocietyId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.CommentFlatOwners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        CommentBy = c.String(),
                        FlatOwnerId = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FlatOwners", t => t.FlatOwnerId)
                .Index(t => t.FlatOwnerId);
            
            CreateTable(
                "dbo.FlatOwnerFamilies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        MobileNo = c.String(),
                        AadhaarCardNo = c.String(),
                        DateOfBirth = c.DateTime(),
                        AdminMember = c.Boolean(nullable: false),
                        ApproverMember = c.Boolean(nullable: false),
                        GenderId = c.Int(nullable: false),
                        RelationshipId = c.Int(nullable: false),
                        FlatOwnerId = c.Long(nullable: false),
                        UserId = c.Long(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.FlatOwners", t => t.FlatOwnerId)
                .ForeignKey("dbo.MasterValues", t => t.GenderId)
                .ForeignKey("dbo.MasterValues", t => t.RelationshipId)
                .Index(t => t.GenderId)
                .Index(t => t.RelationshipId)
                .Index(t => t.FlatOwnerId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CommentFlatOwnerFamilies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        CommentBy = c.String(),
                        FlatOwnerFamilyId = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FlatOwnerFamilies", t => t.FlatOwnerFamilyId)
                .Index(t => t.FlatOwnerFamilyId);
            
            CreateTable(
                "dbo.MapFlatToFlatOwners",
                c => new
                    {
                        FlatId = c.Long(nullable: false),
                        FlatOwnerId = c.Long(nullable: false),
                        MemberSinceDate = c.DateTime(),
                        MemberTillDate = c.DateTime(),
                        FlatOwnerType = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => new { t.FlatId, t.FlatOwnerId })
                .ForeignKey("dbo.Flats", t => t.FlatId)
                .ForeignKey("dbo.FlatOwners", t => t.FlatOwnerId, cascadeDelete: true)
                .Index(t => t.FlatId)
                .Index(t => t.FlatOwnerId);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Make = c.String(),
                        Number = c.String(),
                        Type = c.Byte(),
                        FlatOwnerId = c.Long(nullable: false),
                        FlatId = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Flats", t => t.FlatId)
                .ForeignKey("dbo.FlatOwners", t => t.FlatOwnerId)
                .Index(t => t.FlatOwnerId)
                .Index(t => t.FlatId);
            
            CreateTable(
                "dbo.CommentVehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        CommentBy = c.String(),
                        VehicleId = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId)
                .Index(t => t.VehicleId);
            
            CreateTable(
                "dbo.MaintenanceCosts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DefinitionId = c.Long(nullable: false),
                        Amount = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        FromDate = c.DateTime(),
                        ToDate = c.DateTime(),
                        PerSqrArea = c.Decimal(precision: 18, scale: 2),
                        AllFlats = c.Boolean(nullable: false),
                        SocietyId = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MaintenanceCostDefinitions", t => t.DefinitionId)
                .ForeignKey("dbo.Societies", t => t.SocietyId)
                .Index(t => t.DefinitionId)
                .Index(t => t.SocietyId);
            
            CreateTable(
                "dbo.CommentMCs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        CommentBy = c.String(),
                        MaintenanceCostId = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MaintenanceCosts", t => t.MaintenanceCostId, cascadeDelete: true)
                .Index(t => t.MaintenanceCostId);
            
            CreateTable(
                "dbo.MaintenanceCostDefinitions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        CalculationOnPerSftArea = c.Boolean(nullable: false),
                        For2Wheeler = c.Boolean(nullable: false),
                        For4Wheeler = c.Boolean(nullable: false),
                        FacilityType = c.Byte(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CommentFlats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        CommentBy = c.String(),
                        FlatId = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Flats", t => t.FlatId)
                .Index(t => t.FlatId);
            
            CreateTable(
                "dbo.Floors",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        FacilityId = c.Long(nullable: false),
                        Type = c.Byte(nullable: false),
                        FloorNumber = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Facilities", t => t.FacilityId)
                .Index(t => t.FacilityId);
            
            CreateTable(
                "dbo.SocietyAssets",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        IsUsable = c.Boolean(nullable: false),
                        IsOperational = c.Boolean(nullable: false),
                        Quantity = c.Int(nullable: false),
                        CompanyName = c.String(),
                        Brand = c.String(),
                        PurchaseDate = c.DateTime(),
                        ModelNo = c.String(),
                        SrNo = c.String(),
                        ContactPerson = c.String(),
                        CustomerCareNo = c.String(),
                        FloorId = c.Long(),
                        FacilityId = c.Long(),
                        SocietyId = c.Long(),
                        ComplexId = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Facilities", t => t.FacilityId)
                .ForeignKey("dbo.Floors", t => t.FloorId)
                .ForeignKey("dbo.Societies", t => t.SocietyId)
                .ForeignKey("dbo.Complexes", t => t.ComplexId, cascadeDelete: true)
                .Index(t => t.FloorId)
                .Index(t => t.FacilityId)
                .Index(t => t.SocietyId)
                .Index(t => t.ComplexId);
            
            CreateTable(
                "dbo.MapSocietiesToFacilities",
                c => new
                    {
                        SocietyId = c.Long(nullable: false),
                        FacilityId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.SocietyId, t.FacilityId })
                .ForeignKey("dbo.Facilities", t => t.FacilityId)
                .ForeignKey("dbo.Societies", t => t.SocietyId, cascadeDelete: true)
                .Index(t => t.SocietyId)
                .Index(t => t.FacilityId);
            
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
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Societies", t => t.SocietyId)
                .Index(t => t.SocietyId);
            
            CreateTable(
                "dbo.SocietyStaffs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        PhoneNo = c.String(),
                        AadharCardNo = c.String(),
                        Photo = c.String(),
                        DateOfBirth = c.DateTime(),
                        Address = c.String(),
                        NativeAddress = c.String(),
                        JoiningDate = c.DateTime(nullable: false),
                        LastWorkingDay = c.DateTime(),
                        StaffTypeId = c.Int(),
                        SocietyId = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Societies", t => t.SocietyId)
                .ForeignKey("dbo.MasterValues", t => t.StaffTypeId)
                .Index(t => t.StaffTypeId)
                .Index(t => t.SocietyId);
            
            CreateTable(
                "dbo.BillingLines",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BillId = c.Long(nullable: false),
                        Definition = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BaseAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OtherAmount = c.Decimal(precision: 18, scale: 2),
                        OnArea = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bills", t => t.BillId)
                .Index(t => t.BillId);
            
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        ReceiptNo = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Pending = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FlatId = c.Long(nullable: false),
                        SocietyId = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Flats", t => t.FlatId)
                .ForeignKey("dbo.Societies", t => t.SocietyId)
                .Index(t => t.FlatId)
                .Index(t => t.SocietyId);
            
            CreateTable(
                "dbo.BillingTransactions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        TransactionNo = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        Reference = c.String(),
                        Mode = c.Byte(nullable: false),
                        AuthorizedBy = c.String(),
                        Description = c.String(),
                        ChequeNo = c.String(),
                        Bank = c.String(),
                        Branch = c.String(),
                        ChequeDate = c.DateTime(),
                        FlatId = c.Long(),
                        SocietyId = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        UnApprovedData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Flats", t => t.FlatId)
                .ForeignKey("dbo.Societies", t => t.SocietyId)
                .Index(t => t.FlatId)
                .Index(t => t.SocietyId);
            
            CreateTable(
                "dbo.Instances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BillingTransactions", "SocietyId", "dbo.Societies");
            DropForeignKey("dbo.BillingTransactions", "FlatId", "dbo.Flats");
            DropForeignKey("dbo.Bills", "SocietyId", "dbo.Societies");
            DropForeignKey("dbo.Bills", "FlatId", "dbo.Flats");
            DropForeignKey("dbo.BillingLines", "BillId", "dbo.Bills");
            DropForeignKey("dbo.Attendances", "ShiftTypeId", "dbo.MasterValues");
            DropForeignKey("dbo.MapUserToSocieties", "SocietyId", "dbo.Societies");
            DropForeignKey("dbo.SocietyStaffs", "StaffTypeId", "dbo.MasterValues");
            DropForeignKey("dbo.SocietyStaffs", "SocietyId", "dbo.Societies");
            DropForeignKey("dbo.Attendances", "SocietyStaffId", "dbo.SocietyStaffs");
            DropForeignKey("dbo.SocietyDocuments", "SocietyId", "dbo.Societies");
            DropForeignKey("dbo.SecurityStaffs", "SocietyId", "dbo.Societies");
            DropForeignKey("dbo.MapSocietiesToFacilities", "SocietyId", "dbo.Societies");
            DropForeignKey("dbo.MapUserToComplexes", "ComplexId", "dbo.Complexes");
            DropForeignKey("dbo.Complexes", "StateId", "dbo.MasterValues");
            DropForeignKey("dbo.SocietyAssets", "ComplexId", "dbo.Complexes");
            DropForeignKey("dbo.Societies", "ComplexId", "dbo.Complexes");
            DropForeignKey("dbo.Facilities", "ComplexId", "dbo.Complexes");
            DropForeignKey("dbo.MapSocietiesToFacilities", "FacilityId", "dbo.Facilities");
            DropForeignKey("dbo.SocietyAssets", "SocietyId", "dbo.Societies");
            DropForeignKey("dbo.SocietyAssets", "FloorId", "dbo.Floors");
            DropForeignKey("dbo.SocietyAssets", "FacilityId", "dbo.Facilities");
            DropForeignKey("dbo.Flats", "FloorId", "dbo.Floors");
            DropForeignKey("dbo.FlatParkings", "FloorId", "dbo.Floors");
            DropForeignKey("dbo.Floors", "FacilityId", "dbo.Facilities");
            DropForeignKey("dbo.Flats", "FlatTypeId", "dbo.MasterValues");
            DropForeignKey("dbo.FlatParkings", "FlatId", "dbo.Flats");
            DropForeignKey("dbo.CommentFlats", "FlatId", "dbo.Flats");
            DropForeignKey("dbo.MaintenanceCosts", "SocietyId", "dbo.Societies");
            DropForeignKey("dbo.MapMaintenanceCostsToFlats", "FlatId", "dbo.Flats");
            DropForeignKey("dbo.MapMaintenanceCostsToFlats", "MaintenanceCostId", "dbo.MaintenanceCosts");
            DropForeignKey("dbo.MaintenanceCosts", "DefinitionId", "dbo.MaintenanceCostDefinitions");
            DropForeignKey("dbo.CommentMCs", "MaintenanceCostId", "dbo.MaintenanceCosts");
            DropForeignKey("dbo.DataApprovals", "MaintenanceCostId", "dbo.MaintenanceCosts");
            DropForeignKey("dbo.Vehicles", "FlatOwnerId", "dbo.FlatOwners");
            DropForeignKey("dbo.Vehicles", "FlatId", "dbo.Flats");
            DropForeignKey("dbo.CommentVehicles", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.DataApprovals", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.FlatOwners", "GenderId", "dbo.MasterValues");
            DropForeignKey("dbo.MapFlatToFlatOwners", "FlatOwnerId", "dbo.FlatOwners");
            DropForeignKey("dbo.MapFlatToFlatOwners", "FlatId", "dbo.Flats");
            DropForeignKey("dbo.FlatOwnerFamilies", "RelationshipId", "dbo.MasterValues");
            DropForeignKey("dbo.FlatOwnerFamilies", "GenderId", "dbo.MasterValues");
            DropForeignKey("dbo.FlatOwnerFamilies", "FlatOwnerId", "dbo.FlatOwners");
            DropForeignKey("dbo.CommentFlatOwnerFamilies", "FlatOwnerFamilyId", "dbo.FlatOwnerFamilies");
            DropForeignKey("dbo.DataApprovals", "FlatOwnerFamilyId", "dbo.FlatOwnerFamilies");
            DropForeignKey("dbo.FlatOwnerFamilies", "UserId", "dbo.Users");
            DropForeignKey("dbo.CommentFlatOwners", "FlatOwnerId", "dbo.FlatOwners");
            DropForeignKey("dbo.DataApprovals", "FlatOwnerId", "dbo.FlatOwners");
            DropForeignKey("dbo.FlatOwners", "UserId", "dbo.Users");
            DropForeignKey("dbo.MapUserToSocieties", "UserId", "dbo.Users");
            DropForeignKey("dbo.MapUserToSocieties", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.MapUserToComplexes", "UserId", "dbo.Users");
            DropForeignKey("dbo.MapUserToComplexes", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Roles", "SocietyId", "dbo.Societies");
            DropForeignKey("dbo.Roles", "ComplexId", "dbo.Complexes");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.DataApprovals", "FlatId", "dbo.Flats");
            DropForeignKey("dbo.FlatParkings", "FacilityId", "dbo.Facilities");
            DropForeignKey("dbo.Complexes", "CityId", "dbo.MasterValues");
            DropForeignKey("dbo.SecurityStaffs", "ShiftTypeId", "dbo.MasterValues");
            DropForeignKey("dbo.Attendances", "SecurityStaffId", "dbo.SecurityStaffs");
            DropIndex("dbo.MapMaintenanceCostsToFlats", new[] { "FlatId" });
            DropIndex("dbo.MapMaintenanceCostsToFlats", new[] { "MaintenanceCostId" });
            DropIndex("dbo.BillingTransactions", new[] { "SocietyId" });
            DropIndex("dbo.BillingTransactions", new[] { "FlatId" });
            DropIndex("dbo.Bills", new[] { "SocietyId" });
            DropIndex("dbo.Bills", new[] { "FlatId" });
            DropIndex("dbo.BillingLines", new[] { "BillId" });
            DropIndex("dbo.SocietyStaffs", new[] { "SocietyId" });
            DropIndex("dbo.SocietyStaffs", new[] { "StaffTypeId" });
            DropIndex("dbo.SocietyDocuments", new[] { "SocietyId" });
            DropIndex("dbo.MapSocietiesToFacilities", new[] { "FacilityId" });
            DropIndex("dbo.MapSocietiesToFacilities", new[] { "SocietyId" });
            DropIndex("dbo.SocietyAssets", new[] { "ComplexId" });
            DropIndex("dbo.SocietyAssets", new[] { "SocietyId" });
            DropIndex("dbo.SocietyAssets", new[] { "FacilityId" });
            DropIndex("dbo.SocietyAssets", new[] { "FloorId" });
            DropIndex("dbo.Floors", new[] { "FacilityId" });
            DropIndex("dbo.CommentFlats", new[] { "FlatId" });
            DropIndex("dbo.CommentMCs", new[] { "MaintenanceCostId" });
            DropIndex("dbo.MaintenanceCosts", new[] { "SocietyId" });
            DropIndex("dbo.MaintenanceCosts", new[] { "DefinitionId" });
            DropIndex("dbo.CommentVehicles", new[] { "VehicleId" });
            DropIndex("dbo.Vehicles", new[] { "FlatId" });
            DropIndex("dbo.Vehicles", new[] { "FlatOwnerId" });
            DropIndex("dbo.MapFlatToFlatOwners", new[] { "FlatOwnerId" });
            DropIndex("dbo.MapFlatToFlatOwners", new[] { "FlatId" });
            DropIndex("dbo.CommentFlatOwnerFamilies", new[] { "FlatOwnerFamilyId" });
            DropIndex("dbo.FlatOwnerFamilies", new[] { "UserId" });
            DropIndex("dbo.FlatOwnerFamilies", new[] { "FlatOwnerId" });
            DropIndex("dbo.FlatOwnerFamilies", new[] { "RelationshipId" });
            DropIndex("dbo.FlatOwnerFamilies", new[] { "GenderId" });
            DropIndex("dbo.CommentFlatOwners", new[] { "FlatOwnerId" });
            DropIndex("dbo.MapUserToSocieties", new[] { "RoleId" });
            DropIndex("dbo.MapUserToSocieties", new[] { "SocietyId" });
            DropIndex("dbo.MapUserToSocieties", new[] { "UserId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.Roles", new[] { "ComplexId" });
            DropIndex("dbo.Roles", new[] { "SocietyId" });
            DropIndex("dbo.MapUserToComplexes", new[] { "RoleId" });
            DropIndex("dbo.MapUserToComplexes", new[] { "ComplexId" });
            DropIndex("dbo.MapUserToComplexes", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.FlatOwners", new[] { "UserId" });
            DropIndex("dbo.FlatOwners", new[] { "GenderId" });
            DropIndex("dbo.DataApprovals", new[] { "VehicleId" });
            DropIndex("dbo.DataApprovals", new[] { "FlatOwnerFamilyId" });
            DropIndex("dbo.DataApprovals", new[] { "MaintenanceCostId" });
            DropIndex("dbo.DataApprovals", new[] { "FlatOwnerId" });
            DropIndex("dbo.DataApprovals", new[] { "FlatId" });
            DropIndex("dbo.Flats", new[] { "FlatTypeId" });
            DropIndex("dbo.Flats", new[] { "FloorId" });
            DropIndex("dbo.FlatParkings", new[] { "FlatId" });
            DropIndex("dbo.FlatParkings", new[] { "FloorId" });
            DropIndex("dbo.FlatParkings", new[] { "FacilityId" });
            DropIndex("dbo.Facilities", new[] { "ComplexId" });
            DropIndex("dbo.Complexes", new[] { "StateId" });
            DropIndex("dbo.Complexes", new[] { "CityId" });
            DropIndex("dbo.Societies", new[] { "ComplexId" });
            DropIndex("dbo.SecurityStaffs", new[] { "ShiftTypeId" });
            DropIndex("dbo.SecurityStaffs", new[] { "SocietyId" });
            DropIndex("dbo.Attendances", new[] { "ShiftTypeId" });
            DropIndex("dbo.Attendances", new[] { "SocietyStaffId" });
            DropIndex("dbo.Attendances", new[] { "SecurityStaffId" });
            DropTable("dbo.MapMaintenanceCostsToFlats");
            DropTable("dbo.Instances");
            DropTable("dbo.BillingTransactions");
            DropTable("dbo.Bills");
            DropTable("dbo.BillingLines");
            DropTable("dbo.SocietyStaffs");
            DropTable("dbo.SocietyDocuments");
            DropTable("dbo.MapSocietiesToFacilities");
            DropTable("dbo.SocietyAssets");
            DropTable("dbo.Floors");
            DropTable("dbo.CommentFlats");
            DropTable("dbo.MaintenanceCostDefinitions");
            DropTable("dbo.CommentMCs");
            DropTable("dbo.MaintenanceCosts");
            DropTable("dbo.CommentVehicles");
            DropTable("dbo.Vehicles");
            DropTable("dbo.MapFlatToFlatOwners");
            DropTable("dbo.CommentFlatOwnerFamilies");
            DropTable("dbo.FlatOwnerFamilies");
            DropTable("dbo.CommentFlatOwners");
            DropTable("dbo.MapUserToSocieties");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Roles");
            DropTable("dbo.MapUserToComplexes");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.FlatOwners");
            DropTable("dbo.DataApprovals");
            DropTable("dbo.Flats");
            DropTable("dbo.FlatParkings");
            DropTable("dbo.Facilities");
            DropTable("dbo.Complexes");
            DropTable("dbo.Societies");
            DropTable("dbo.MasterValues");
            DropTable("dbo.SecurityStaffs");
            DropTable("dbo.Attendances");
            DropTable("dbo.AllIndiaPincodes");
        }
    }
}
