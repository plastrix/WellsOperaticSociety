namespace WellsOperaticSociety.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChnageFunctionidFromStringToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MemberRolesInShows", "FunctionId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MemberRolesInShows", "FunctionId", c => c.String());
        }
    }
}
