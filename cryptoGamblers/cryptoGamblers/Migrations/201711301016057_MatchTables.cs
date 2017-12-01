namespace cryptoGamblers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MatchTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AfterMatches",
                c => new
                    {
                        AfterMatchId = c.Int(nullable: false, identity: true),
                        GameDate = c.DateTime(nullable: false),
                        GameWinner = c.String(),
                    })
                .PrimaryKey(t => t.AfterMatchId);
            
            CreateTable(
                "dbo.Matches",
                c => new
                    {
                        MatchId = c.Int(nullable: false, identity: true),
                        Opponent1 = c.String(),
                        Opponent2 = c.String(),
                    })
                .PrimaryKey(t => t.MatchId);
            
            CreateTable(
                "dbo.QueueIns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Opponent1 = c.String(),
                        Opponent2 = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.QueueIns");
            DropTable("dbo.Matches");
            DropTable("dbo.AfterMatches");
        }
    }
}
