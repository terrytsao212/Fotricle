namespace Fotricle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeFeedbacktype : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OpenTimes", "OpenDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.FeedBacks", "Food", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.FeedBacks", "Service", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.FeedBacks", "AllSuggest", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.OpenTimes", "SDateTime", c => c.DateTime());
            AlterColumn("dbo.OpenTimes", "EDateTimeDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OpenTimes", "EDateTimeDate", c => c.String());
            AlterColumn("dbo.OpenTimes", "SDateTime", c => c.String());
            AlterColumn("dbo.FeedBacks", "AllSuggest", c => c.String(maxLength: 50));
            AlterColumn("dbo.FeedBacks", "Service", c => c.String(maxLength: 50));
            AlterColumn("dbo.FeedBacks", "Food", c => c.String(maxLength: 50));
            DropColumn("dbo.OpenTimes", "OpenDate");
        }
    }
}
