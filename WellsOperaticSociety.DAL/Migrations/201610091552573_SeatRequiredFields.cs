namespace WellsOperaticSociety.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeatRequiredFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Seats", "SeatNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Seats", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Seats", "Description", c => c.String());
            AlterColumn("dbo.Seats", "SeatNumber", c => c.String());
        }
    }
}
