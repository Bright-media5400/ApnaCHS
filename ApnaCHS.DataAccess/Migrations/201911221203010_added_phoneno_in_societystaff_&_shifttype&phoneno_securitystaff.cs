namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_phoneno_in_societystaff__shifttypephoneno_securitystaff : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SecurityStaffs", "PhoneNo", c => c.String());
            AddColumn("dbo.SecurityStaffs", "ShiftTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.SocietyStaffs", "PhoneNo", c => c.String());
            CreateIndex("dbo.SecurityStaffs", "ShiftTypeId");
            AddForeignKey("dbo.SecurityStaffs", "ShiftTypeId", "dbo.MasterValues", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SecurityStaffs", "ShiftTypeId", "dbo.MasterValues");
            DropIndex("dbo.SecurityStaffs", new[] { "ShiftTypeId" });
            DropColumn("dbo.SocietyStaffs", "PhoneNo");
            DropColumn("dbo.SecurityStaffs", "ShiftTypeId");
            DropColumn("dbo.SecurityStaffs", "PhoneNo");
        }
    }
}
