namespace cryptoGamblers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Winstreakandwinstreakmax : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "WinStreak", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "WinStreakMax", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "WinStreakMax");
            DropColumn("dbo.AspNetUsers", "WinStreak");
        }
    }
}
