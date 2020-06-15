namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bill_Note : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "Note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bills", "Note");
        }
    }
}
