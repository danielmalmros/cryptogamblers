namespace cryptoGamblers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dicerolldataaddedtomatchdata : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MatchDatas", "Opponent1Roll", c => c.Int(nullable: false));
            AddColumn("dbo.MatchDatas", "Opponent2Roll", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MatchDatas", "Opponent2Roll");
            DropColumn("dbo.MatchDatas", "Opponent1Roll");
        }
    }
}
