namespace Fotricle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLongitude : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 200),
                        Password = c.String(nullable: false, maxLength: 100),
                        PasswordSalt = c.String(),
                        BrandName = c.String(maxLength: 50),
                        BrandStory = c.String(maxLength: 200),
                        PhoneNumber = c.String(maxLength: 50),
                        Address = c.String(maxLength: 100),
                        Vip = c.Int(nullable: false),
                        Sort = c.Int(nullable: false),
                        LogoPhoto = c.String(),
                        CarImage = c.String(),
                        Status = c.Int(nullable: false),
                        LinePay = c.String(),
                        QrCode = c.String(),
                        FbAccount = c.String(nullable: false, maxLength: 100),
                        Verification = c.Int(nullable: false),
                        Score = c.String(maxLength: 50),
                        InitDate = c.DateTime(),
                        Permission = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OpenTimes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BrandId = c.Int(),
                        OpenDate = c.DateTime(nullable: false),
                        Date = c.String(),
                        Status = c.Int(nullable: false),
                        SDateTime = c.DateTime(),
                        EDateTimeDate = c.DateTime(),
                        Location = c.String(maxLength: 50),
                        Longitude = c.String(),
                        Latitude = c.String(maxLength: 50),
                        InitDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Brands", t => t.BrandId)
                .Index(t => t.BrandId);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        ProductListId = c.Int(nullable: false),
                        BrandId = c.Int(nullable: false),
                        BrandName = c.String(),
                        ProductPhoto = c.String(),
                        ProductName = c.String(),
                        ProductUnit = c.Int(nullable: false),
                        ProductPrice = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        InitDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 50),
                        UserName = c.String(maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 100),
                        PasswordSalt = c.String(),
                        CusPhone = c.String(),
                        Gender = c.Int(nullable: false),
                        Age = c.String(),
                        CusPhoto = c.String(),
                        Abandon = c.String(),
                        Blacklist = c.Int(nullable: false),
                        InitDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FeedBacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Guid = c.String(),
                        OrderId = c.Int(),
                        CustomerId = c.Int(nullable: false),
                        Food = c.Double(nullable: false),
                        Service = c.Double(nullable: false),
                        AllSuggest = c.Double(nullable: false),
                        CarSuggest = c.String(maxLength: 100),
                        InitDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .Index(t => t.OrderId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BrandId = c.Int(),
                        CustomerId = c.Int(),
                        OrderStatus = c.Int(nullable: false),
                        OrderTime = c.DateTime(nullable: false),
                        Payment = c.Int(nullable: false),
                        OrderNumber = c.String(),
                        MealNumber = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        LinepayVer = c.String(),
                        Site = c.Int(nullable: false),
                        CompleteTime = c.DateTime(),
                        Remarks = c.String(maxLength: 100),
                        Remark1 = c.String(maxLength: 100),
                        Remark2 = c.String(maxLength: 100),
                        Remark3 = c.String(maxLength: 100),
                        Remark4 = c.String(maxLength: 100),
                        InitDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Brands", t => t.BrandId)
                .Index(t => t.BrandId);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(),
                        ProductListId = c.Int(),
                        ProductName = c.String(),
                        ProductPrice = c.Int(nullable: false),
                        ProductUnit = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        InitDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .ForeignKey("dbo.ProductLists", t => t.ProductListId)
                .Index(t => t.OrderId)
                .Index(t => t.ProductListId);
            
            CreateTable(
                "dbo.ProductLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BrandId = c.Int(),
                        ProductSort = c.Int(nullable: false),
                        ProductName = c.String(nullable: false, maxLength: 50),
                        Price = c.Int(nullable: false),
                        Unit = c.String(nullable: false),
                        ProductPhoto = c.String(),
                        Total = c.Int(nullable: false),
                        ProductDetail = c.String(nullable: false, maxLength: 100),
                        Discount = c.Int(nullable: false),
                        IsUse = c.Int(nullable: false),
                        InitDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Brands", t => t.BrandId)
                .Index(t => t.BrandId);
            
            CreateTable(
                "dbo.MyFollows",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BrandId = c.Int(),
                        BrandName = c.String(maxLength: 50),
                        CustomerId = c.Int(),
                        InitDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Brands", t => t.BrandId)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.BrandId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Notices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(),
                        OrderId = c.Int(),
                        OrderStatus = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 50),
                        IsRead = c.Int(nullable: false),
                        InitDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .Index(t => t.CustomerId)
                .Index(t => t.OrderId);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VipOrders", "BrandId", "dbo.Brands");
            DropForeignKey("dbo.Notices", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Notices", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.MyFollows", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.MyFollows", "BrandId", "dbo.Brands");
            DropForeignKey("dbo.FeedBacks", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderDetails", "ProductListId", "dbo.ProductLists");
            DropForeignKey("dbo.ProductLists", "BrandId", "dbo.Brands");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "BrandId", "dbo.Brands");
            DropForeignKey("dbo.FeedBacks", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.OpenTimes", "BrandId", "dbo.Brands");
            DropIndex("dbo.VipOrders", new[] { "BrandId" });
            DropIndex("dbo.Notices", new[] { "OrderId" });
            DropIndex("dbo.Notices", new[] { "CustomerId" });
            DropIndex("dbo.MyFollows", new[] { "CustomerId" });
            DropIndex("dbo.MyFollows", new[] { "BrandId" });
            DropIndex("dbo.ProductLists", new[] { "BrandId" });
            DropIndex("dbo.OrderDetails", new[] { "ProductListId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "BrandId" });
            DropIndex("dbo.FeedBacks", new[] { "CustomerId" });
            DropIndex("dbo.FeedBacks", new[] { "OrderId" });
            DropIndex("dbo.OpenTimes", new[] { "BrandId" });
            DropTable("dbo.VipOrders");
            DropTable("dbo.Notices");
            DropTable("dbo.MyFollows");
            DropTable("dbo.ProductLists");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Orders");
            DropTable("dbo.FeedBacks");
            DropTable("dbo.Customers");
            DropTable("dbo.Carts");
            DropTable("dbo.OpenTimes");
            DropTable("dbo.Brands");
        }
    }
}
