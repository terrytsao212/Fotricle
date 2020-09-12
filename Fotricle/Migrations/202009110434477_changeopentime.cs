namespace Fotricle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeopentime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "OrderTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "OrderTime", c => c.DateTime());
        }
    }
}
