namespace Fotricle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeOpentimetype : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OpenTimes", "OpenDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.FeedBacks", "Food", c => c.Int(nullable: false));
            AlterColumn("dbo.FeedBacks", "Service", c => c.Int(nullable: false));
            AlterColumn("dbo.FeedBacks", "AllSuggest", c => c.Int(nullable: false));
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
