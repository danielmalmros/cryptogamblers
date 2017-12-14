namespace cryptoGamblers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testifresultisreturnedtoopponents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MatchDatas", "rollReturnedOpponent1", c => c.Boolean(nullable: false));
            AddColumn("dbo.MatchDatas", "rollReturnedOpponent2", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MatchDatas", "rollReturnedOpponent2");
            DropColumn("dbo.MatchDatas", "rollReturnedOpponent1");
        }
    }
}
