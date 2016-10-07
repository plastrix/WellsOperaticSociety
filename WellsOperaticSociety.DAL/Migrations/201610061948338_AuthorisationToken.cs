namespace WellsOperaticSociety.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuthorisationToken : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthorisationTokens",
                c => new
                    {
                        AuthorisationTokenId = c.Int(nullable: false, identity: true),
                        Token = c.String(),
                        Member = c.Int(nullable: false),
                        Used = c.Boolean(nullable: false),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AuthorisationTokenId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AuthorisationTokens");
        }
    }
}
