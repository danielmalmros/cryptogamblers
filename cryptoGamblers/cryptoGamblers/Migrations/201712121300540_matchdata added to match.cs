namespace cryptoGamblers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class matchdataaddedtomatch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MatchDatas",
                c => new
                    {
                        MatchDataId = c.Int(nullable: false, identity: true),
                        PrizePool = c.Int(nullable: false),
                        Accepted = c.Boolean(nullable: false),
                        MatchId = c.Int(),
                    })
                .PrimaryKey(t => t.MatchDataId)
                .ForeignKey("dbo.Matches", t => t.MatchId)
                .Index(t => t.MatchId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MatchDatas", "MatchId", "dbo.Matches");
            DropIndex("dbo.MatchDatas", new[] { "MatchId" });
            DropTable("dbo.MatchDatas");
        }
    }
}
