namespace WellsOperaticSociety.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFunctionIdToMemberRolesInShow : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemberRolesInShows", "FunctionId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MemberRolesInShows", "FunctionId");
        }
    }
}
