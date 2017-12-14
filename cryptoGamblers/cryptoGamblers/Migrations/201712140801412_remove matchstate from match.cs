namespace cryptoGamblers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removematchstatefrommatch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MatchDatas", "MatchState", c => c.Int(nullable: false));
            DropColumn("dbo.Matches", "MatchState");
            DropColumn("dbo.MatchDatas", "BetState");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MatchDatas", "BetState", c => c.Int(nullable: false));
            AddColumn("dbo.Matches", "MatchState", c => c.Int(nullable: false));
            DropColumn("dbo.MatchDatas", "MatchState");
        }
    }
}
