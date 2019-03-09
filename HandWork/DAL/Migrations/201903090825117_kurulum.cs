namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kurulum : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Notifications", name: "ReceiverId", newName: "Owner_Id");
            RenameIndex(table: "dbo.Notifications", name: "IX_ReceiverId", newName: "IX_Owner_Id");
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Content = c.String(),
                        SenderID = c.String(),
                        ReceiverID = c.String(),
                        Owner_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id, cascadeDelete: true)
                .Index(t => t.Owner_Id);
            
            AddColumn("dbo.Notifications", "SenderMemId", c => c.String());
            AddColumn("dbo.Notifications", "ReceiverMemId", c => c.String());
            DropColumn("dbo.Notifications", "SenderId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "SenderId", c => c.String());
            DropForeignKey("dbo.Messages", "Owner_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "Owner_Id" });
            DropColumn("dbo.Notifications", "ReceiverMemId");
            DropColumn("dbo.Notifications", "SenderMemId");
            DropTable("dbo.Messages");
            RenameIndex(table: "dbo.Notifications", name: "IX_Owner_Id", newName: "IX_ReceiverId");
            RenameColumn(table: "dbo.Notifications", name: "Owner_Id", newName: "ReceiverId");
        }
    }
}
