namespace ApnaCHS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_Flat_Balcony : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Flats", "Balcony", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Flats", "Balcony");
        }
    }
}
