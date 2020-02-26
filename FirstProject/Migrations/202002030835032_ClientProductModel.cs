namespace FirstProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientProductModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientProducts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserTypeId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.UserTypes", t => t.UserTypeId, cascadeDelete: true)
                .Index(t => t.UserTypeId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientProducts", "UserTypeId", "dbo.UserTypes");
            DropForeignKey("dbo.ClientProducts", "ProductId", "dbo.Products");
            DropIndex("dbo.ClientProducts", new[] { "ProductId" });
            DropIndex("dbo.ClientProducts", new[] { "UserTypeId" });
            DropTable("dbo.ClientProducts");
        }
    }
}
