namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kurulum : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Baskets",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Member_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.Member_Id)
                .Index(t => t.Member_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        NameSurname = c.String(),
                        Gender = c.Int(nullable: false),
                        Adress = c.String(),
                        Age = c.Int(nullable: false),
                        Text = c.String(),
                        ComplaintCount = c.Int(nullable: false),
                        FavorCount = c.Int(nullable: false),
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
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Member_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.Member_Id, cascadeDelete: true)
                .Index(t => t.Member_Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        Member_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.Member_Id)
                .Index(t => t.Member_Id);
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemCount = c.Int(nullable: false),
                        Order_ID = c.Int(),
                        Product_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Orders", t => t.Order_ID)
                .ForeignKey("dbo.Products", t => t.Product_ID, cascadeDelete: true)
                .Index(t => t.Order_ID)
                .Index(t => t.Product_ID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        Content = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StockCount = c.Int(nullable: false),
                        LikeCount = c.Int(nullable: false),
                        DisLikeCount = c.Int(nullable: false),
                        Member_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.Member_Id, cascadeDelete: true)
                .Index(t => t.Member_Id);
            
            CreateTable(
                "dbo.ProductImages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ImageURL = c.String(),
                        Product_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.Product_ID, cascadeDelete: true)
                .Index(t => t.Product_ID);
            
            CreateTable(
                "dbo.ProductItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemCount = c.Int(nullable: false),
                        Product_ID = c.Int(nullable: false),
                        Basket_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.Product_ID, cascadeDelete: true)
                .ForeignKey("dbo.Baskets", t => t.Basket_ID, cascadeDelete: true)
                .Index(t => t.Product_ID)
                .Index(t => t.Basket_ID);
            
            CreateTable(
                "dbo.ProfilPhotoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ImageURL = c.String(),
                        Member_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.Member_Id)
                .Index(t => t.Member_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ProductItems", "Basket_ID", "dbo.Baskets");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProfilPhotoes", "Member_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductItems", "Product_ID", "dbo.Products");
            DropForeignKey("dbo.ProductImages", "Product_ID", "dbo.Products");
            DropForeignKey("dbo.OrderItems", "Product_ID", "dbo.Products");
            DropForeignKey("dbo.Products", "Member_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrderItems", "Order_ID", "dbo.Orders");
            DropForeignKey("dbo.Orders", "Member_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "Member_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Baskets", "Member_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.ProfilPhotoes", new[] { "Member_Id" });
            DropIndex("dbo.ProductItems", new[] { "Basket_ID" });
            DropIndex("dbo.ProductItems", new[] { "Product_ID" });
            DropIndex("dbo.ProductImages", new[] { "Product_ID" });
            DropIndex("dbo.Products", new[] { "Member_Id" });
            DropIndex("dbo.OrderItems", new[] { "Product_ID" });
            DropIndex("dbo.OrderItems", new[] { "Order_ID" });
            DropIndex("dbo.Orders", new[] { "Member_Id" });
            DropIndex("dbo.Notifications", new[] { "Member_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Baskets", new[] { "Member_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.ProfilPhotoes");
            DropTable("dbo.ProductItems");
            DropTable("dbo.ProductImages");
            DropTable("dbo.Products");
            DropTable("dbo.OrderItems");
            DropTable("dbo.Orders");
            DropTable("dbo.Notifications");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Baskets");
        }
    }
}
