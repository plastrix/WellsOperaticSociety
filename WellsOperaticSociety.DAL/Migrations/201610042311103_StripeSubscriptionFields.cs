namespace WellsOperaticSociety.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StripeSubscriptionFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Memberships", "CancelAtEnd", c => c.Boolean(nullable: false));
            AddColumn("dbo.Memberships", "StripeSubscriptionId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Memberships", "StripeSubscriptionId");
            DropColumn("dbo.Memberships", "CancelAtEnd");
        }
    }
}
