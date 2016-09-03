namespace WellsOperaticSociety.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubscriptionAndMembershipType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Memberships", "IsSubscription", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Memberships", "IsSubscription");
        }
    }
}
