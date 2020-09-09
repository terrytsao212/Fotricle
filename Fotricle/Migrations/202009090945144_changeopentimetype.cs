namespace Fotricle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeopentimetype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "MealNumber", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.OpenTimes", "SDateTime", c => c.String());
            AlterColumn("dbo.OpenTimes", "EDateTimeDate", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OpenTimes", "EDateTimeDate", c => c.DateTime());
            AlterColumn("dbo.OpenTimes", "SDateTime", c => c.DateTime());
            AlterColumn("dbo.Orders", "MealNumber", c => c.Int(nullable: false));
        }
    }
}
