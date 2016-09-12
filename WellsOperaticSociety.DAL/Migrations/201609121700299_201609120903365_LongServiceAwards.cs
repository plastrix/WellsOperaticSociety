namespace WellsOperaticSociety.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201609120903365_LongServiceAwards : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LongServiceAwards", "Awarded", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LongServiceAwards", "Awarded");
        }
    }
}
