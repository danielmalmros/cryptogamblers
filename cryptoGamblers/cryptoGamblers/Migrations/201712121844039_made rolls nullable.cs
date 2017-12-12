namespace cryptoGamblers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class maderollsnullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MatchDatas", "Opponent1Roll", c => c.Int());
            AlterColumn("dbo.MatchDatas", "Opponent2Roll", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MatchDatas", "Opponent2Roll", c => c.Int(nullable: false));
            AlterColumn("dbo.MatchDatas", "Opponent1Roll", c => c.Int(nullable: false));
        }
    }
}
