namespace cryptoGamblers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateaddedtomatch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Matches", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Matches", "Date");
        }
    }
}
