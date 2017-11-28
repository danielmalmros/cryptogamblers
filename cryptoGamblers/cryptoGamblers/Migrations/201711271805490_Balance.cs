namespace cryptoGamblers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Balance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Balance", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Balance");
        }
    }
}
