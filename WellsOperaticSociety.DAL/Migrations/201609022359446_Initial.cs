namespace WellsOperaticSociety.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MemberRolesInShows",
                c => new
                    {
                        MemberRolesInShowId = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        Group = c.String(),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.MemberRolesInShowId);
            
            CreateTable(
                "dbo.Memberships",
                c => new
                    {
                        MembershipId = c.Int(nullable: false, identity: true),
                        Member = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        MembershipType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MembershipId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Memberships");
            DropTable("dbo.MemberRolesInShows");
        }
    }
}
