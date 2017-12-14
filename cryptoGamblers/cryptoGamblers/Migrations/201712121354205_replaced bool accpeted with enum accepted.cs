namespace cryptoGamblers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class replacedboolaccpetedwithenumaccepted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MatchDatas", "betState", c => c.Int(nullable: false));
            DropColumn("dbo.MatchDatas", "Accepted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MatchDatas", "Accepted", c => c.Boolean(nullable: false));
            DropColumn("dbo.MatchDatas", "betState");
        }
    }
}
