namespace WellsOperaticSociety.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableAddedToModelMemberInSHow : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MemberRolesInShows", "MemberId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MemberRolesInShows", "MemberId", c => c.Int(nullable: false));
        }
    }
}
