namespace WellsOperaticSociety.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedRequirements : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MemberRolesInShows", "MemberId", c => c.Int(nullable: false));
            AlterColumn("dbo.MemberRolesInShows", "Group", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MemberRolesInShows", "Group", c => c.String());
            AlterColumn("dbo.MemberRolesInShows", "MemberId", c => c.Int());
        }
    }
}
