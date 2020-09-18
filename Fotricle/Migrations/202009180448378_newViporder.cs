namespace Fotricle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newViporder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VipOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BrandId = c.Int(nullable: false),
                        Status = c.String(),
                        Message = c.String(),
                        Remark = c.String(),
                        InitDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Brands", t => t.BrandId, cascadeDelete: true)
                .Index(t => t.BrandId);
            
            AddColumn("dbo.Notices", "IsRead", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VipOrders", "BrandId", "dbo.Brands");
            DropIndex("dbo.VipOrders", new[] { "BrandId" });
            DropColumn("dbo.Notices", "IsRead");
            DropTable("dbo.VipOrders");
        }
    }
}
