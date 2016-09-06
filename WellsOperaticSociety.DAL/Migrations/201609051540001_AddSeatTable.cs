namespace WellsOperaticSociety.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSeatTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Seats",
                c => new
                    {
                        SeatId = c.Int(nullable: false, identity: true),
                        SeatNumber = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.SeatId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Seats");
        }
    }
}
