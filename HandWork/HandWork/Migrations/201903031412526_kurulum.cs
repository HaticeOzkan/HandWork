namespace HandWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kurulum : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        Customer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.RoleId)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TC = c.Int(nullable: false),
                        NameSurname = c.String(),
                        Gender = c.Int(nullable: false),
                        Adress = c.String(),
                        Age = c.Int(nullable: false),
                        RegisteryDate = c.DateTime(nullable: false),
                        ProfilPhotoURL = c.String(),
                        ComplaintCount = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        SendToEmail = c.String(),
                        Discriminator = c.String(maxLength: 128),
                        MyProductCart_ID = c.Int(),
                        ShoppingCart_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MyProductCarts", t => t.MyProductCart_ID)
                .ForeignKey("dbo.ShoppingCarts", t => t.ShoppingCart_ID)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.MyProductCart_ID)
                .Index(t => t.ShoppingCart_ID);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        Customer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Customer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.MyProductCarts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitStock = c.Int(nullable: false),
                        ProductName = c.String(),
                        Description = c.String(),
                        LikeCount = c.Int(nullable: false),
                        DisLikeCount = c.Int(nullable: false),
                        MyProductCart_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.MyProductCarts", t => t.MyProductCart_ID)
                .Index(t => t.MyProductCart_ID);
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Count = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Order_ID = c.Int(),
                        Product_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Orders", t => t.Order_ID)
                .ForeignKey("dbo.Products", t => t.Product_ID)
                .Index(t => t.Order_ID)
                .Index(t => t.Product_ID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        Paid = c.Boolean(nullable: false),
                        Customer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.ShoppingCarts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductImages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LargeImageURL = c.String(),
                        ThumbImageURL = c.String(),
                        Product_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.Product_ID)
                .Index(t => t.Product_ID);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TagName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ShoppingCartProducts",
                c => new
                    {
                        ShoppingCart_ID = c.Int(nullable: false),
                        Product_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ShoppingCart_ID, t.Product_ID })
                .ForeignKey("dbo.ShoppingCarts", t => t.ShoppingCart_ID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_ID, cascadeDelete: true)
                .Index(t => t.ShoppingCart_ID)
                .Index(t => t.Product_ID);
            
            CreateTable(
                "dbo.TagProducts",
                c => new
                    {
                        Tag_ID = c.Int(nullable: false),
                        Product_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_ID, t.Product_ID })
                .ForeignKey("dbo.Tags", t => t.Tag_ID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_ID, cascadeDelete: true)
                .Index(t => t.Tag_ID)
                .Index(t => t.Product_ID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Id", "dbo.Customers");
            DropForeignKey("dbo.TagProducts", "Product_ID", "dbo.Products");
            DropForeignKey("dbo.TagProducts", "Tag_ID", "dbo.Tags");
            DropForeignKey("dbo.ProductImages", "Product_ID", "dbo.Products");
            DropForeignKey("dbo.OrderItems", "Product_ID", "dbo.Products");
            DropForeignKey("dbo.OrderItems", "Order_ID", "dbo.Orders");
            DropForeignKey("dbo.Customers", "ShoppingCart_ID", "dbo.ShoppingCarts");
            DropForeignKey("dbo.ShoppingCartProducts", "Product_ID", "dbo.Products");
            DropForeignKey("dbo.ShoppingCartProducts", "ShoppingCart_ID", "dbo.ShoppingCarts");
            DropForeignKey("dbo.AspNetUserRoles", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Orders", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Customers", "MyProductCart_ID", "dbo.MyProductCarts");
            DropForeignKey("dbo.AspNetUserLogins", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.AspNetUserClaims", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Products", "MyProductCart_ID", "dbo.MyProductCarts");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUsers", new[] { "Id" });
            DropIndex("dbo.TagProducts", new[] { "Product_ID" });
            DropIndex("dbo.TagProducts", new[] { "Tag_ID" });
            DropIndex("dbo.ShoppingCartProducts", new[] { "Product_ID" });
            DropIndex("dbo.ShoppingCartProducts", new[] { "ShoppingCart_ID" });
            DropIndex("dbo.ProductImages", new[] { "Product_ID" });
            DropIndex("dbo.Orders", new[] { "Customer_Id" });
            DropIndex("dbo.OrderItems", new[] { "Product_ID" });
            DropIndex("dbo.OrderItems", new[] { "Order_ID" });
            DropIndex("dbo.Products", new[] { "MyProductCart_ID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "Customer_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "Customer_Id" });
            DropIndex("dbo.Customers", new[] { "ShoppingCart_ID" });
            DropIndex("dbo.Customers", new[] { "MyProductCart_ID" });
            DropIndex("dbo.Customers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "Customer_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.TagProducts");
            DropTable("dbo.ShoppingCartProducts");
            DropTable("dbo.Tags");
            DropTable("dbo.ProductImages");
            DropTable("dbo.ShoppingCarts");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderItems");
            DropTable("dbo.Products");
            DropTable("dbo.MyProductCarts");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Customers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
        }
    }
}
