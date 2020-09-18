namespace Fotricle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcustomer : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.FeedBacks", "CustomerId");
            AddForeignKey("dbo.FeedBacks", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FeedBacks", "CustomerId", "dbo.Customers");
            DropIndex("dbo.FeedBacks", new[] { "CustomerId" });
        }
    }
}
