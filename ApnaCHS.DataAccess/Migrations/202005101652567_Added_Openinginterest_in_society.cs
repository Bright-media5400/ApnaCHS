namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Openinginterest_in_society : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Societies", "OpeningInterest", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Societies", "OpeningInterest");
        }
    }
}
