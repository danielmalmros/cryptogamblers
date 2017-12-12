namespace cryptoGamblers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class matchstateadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Matches", "MatchState", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Matches", "MatchState");
        }
    }
}
