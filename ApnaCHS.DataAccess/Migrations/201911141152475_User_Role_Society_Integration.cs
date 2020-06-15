namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_Role_Society_Integration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "InstanceId", "dbo.Instances");
            DropIndex("dbo.Users", new[] { "InstanceId" });
            CreateTable(
                "dbo.MapUsersToSocieties",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        SocietyId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.SocietyId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Societies", t => t.SocietyId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.SocietyId);
            
            AddColumn("dbo.Roles", "SocietyId", c => c.Long());
            AddColumn("dbo.Users", "IsBack", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Roles", "SocietyId");
            AddForeignKey("dbo.Roles", "SocietyId", "dbo.Societies", "Id");
            DropColumn("dbo.Users", "InstanceId");
            DropColumn("dbo.Users", "WhatsappNumber");
            DropColumn("dbo.Users", "bExpired");
            DropColumn("dbo.Users", "ExpireMinutes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "ExpireMinutes", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "bExpired", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "WhatsappNumber", c => c.String());
            AddColumn("dbo.Users", "InstanceId", c => c.Int());
            DropForeignKey("dbo.Roles", "SocietyId", "dbo.Societies");
            DropForeignKey("dbo.MapUsersToSocieties", "SocietyId", "dbo.Societies");
            DropForeignKey("dbo.MapUsersToSocieties", "UserId", "dbo.Users");
            DropIndex("dbo.MapUsersToSocieties", new[] { "SocietyId" });
            DropIndex("dbo.MapUsersToSocieties", new[] { "UserId" });
            DropIndex("dbo.Roles", new[] { "SocietyId" });
            DropColumn("dbo.Users", "IsBack");
            DropColumn("dbo.Roles", "SocietyId");
            DropTable("dbo.MapUsersToSocieties");
            CreateIndex("dbo.Users", "InstanceId");
            AddForeignKey("dbo.Users", "InstanceId", "dbo.Instances", "Id");
        }
    }
}
