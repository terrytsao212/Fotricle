namespace Fotricle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeFeedbacktodouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FeedBacks", "Food", c => c.Double(nullable: false));
            AlterColumn("dbo.FeedBacks", "Service", c => c.Double(nullable: false));
            AlterColumn("dbo.FeedBacks", "AllSuggest", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FeedBacks", "AllSuggest", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.FeedBacks", "Service", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.FeedBacks", "Food", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
