namespace WellsOperaticSociety.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LongServiceAwards : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LongServiceAwards",
                c => new
                    {
                        LongServiceAwardId = c.Int(nullable: false, identity: true),
                        Member = c.Int(nullable: false),
                        Award = c.Int(nullable: false),
                        Hide = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LongServiceAwardId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LongServiceAwards");
        }
    }
}
