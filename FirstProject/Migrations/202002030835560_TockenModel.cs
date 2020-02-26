namespace FirstProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TockenModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tockens",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ComplainDescription = c.String(nullable: false),
                        SolvedDescription = c.String(),
                        Type = c.Boolean(nullable: false),
                        Status = c.Boolean(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tockens", "ProductId", "dbo.Products");
            DropIndex("dbo.Tockens", new[] { "ProductId" });
            DropTable("dbo.Tockens");
        }
    }
}
