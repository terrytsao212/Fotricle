namespace Fotricle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditCart : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Carts", "BrandId", "dbo.Brands");
            DropForeignKey("dbo.Carts", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Carts", "ProductListId", "dbo.ProductLists");
            DropIndex("dbo.Carts", new[] { "CustomerId" });
            DropIndex("dbo.Carts", new[] { "ProductListId" });
            DropIndex("dbo.Carts", new[] { "BrandId" });
            AddColumn("dbo.Carts", "BrandName", c => c.String());
            AlterColumn("dbo.Carts", "CustomerId", c => c.Int(nullable: false));
            AlterColumn("dbo.Carts", "ProductListId", c => c.Int(nullable: false));
            AlterColumn("dbo.Carts", "BrandId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Carts", "BrandId", c => c.Int());
            AlterColumn("dbo.Carts", "ProductListId", c => c.Int());
            AlterColumn("dbo.Carts", "CustomerId", c => c.Int());
            DropColumn("dbo.Carts", "BrandName");
            CreateIndex("dbo.Carts", "BrandId");
            CreateIndex("dbo.Carts", "ProductListId");
            CreateIndex("dbo.Carts", "CustomerId");
            AddForeignKey("dbo.Carts", "ProductListId", "dbo.ProductLists", "Id");
            AddForeignKey("dbo.Carts", "CustomerId", "dbo.Customers", "Id");
            AddForeignKey("dbo.Carts", "BrandId", "dbo.Brands", "Id");
        }
    }
}
