namespace WellsOperaticSociety.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vouchers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vouchers",
                c => new
                    {
                        VoucherId = c.Int(nullable: false, identity: true),
                        FunctionId = c.Int(nullable: false),
                        MemberId = c.Int(nullable: false),
                        Key = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.VoucherId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Vouchers");
        }
    }
}
