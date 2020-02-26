namespace FirstProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientInfoModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClientProducts", "UserTypeId", "dbo.UserTypes");
            DropForeignKey("dbo.SupportProducts", "UserTypeId", "dbo.UserTypes");
            DropForeignKey("dbo.Users", "UserTypeId", "dbo.UserTypes");
            DropIndex("dbo.ClientProducts", new[] { "UserTypeId" });
            DropIndex("dbo.SupportProducts", new[] { "UserTypeId" });
            DropIndex("dbo.Users", new[] { "UserTypeId" });
            CreateTable(
                "dbo.ClientInfoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Contact = c.String(),
                        UsersId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UsersId, cascadeDelete: true)
                .Index(t => t.UsersId);
            
            AddColumn("dbo.ClientProducts", "UsersId", c => c.Int(nullable: false));
            AddColumn("dbo.SupportProducts", "UsersId", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "UserType", c => c.String(nullable: false));
            CreateIndex("dbo.ClientProducts", "UsersId");
            CreateIndex("dbo.SupportProducts", "UsersId");
            AddForeignKey("dbo.ClientProducts", "UsersId", "dbo.Users", "ID", cascadeDelete: true);
            AddForeignKey("dbo.SupportProducts", "UsersId", "dbo.Users", "ID", cascadeDelete: true);
            DropColumn("dbo.ClientProducts", "UserTypeId");
            DropColumn("dbo.SupportProducts", "UserTypeId");
            DropColumn("dbo.Users", "UserTypeId");
            DropTable("dbo.UserTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserRoleName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Users", "UserTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.SupportProducts", "UserTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.ClientProducts", "UserTypeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.SupportProducts", "UsersId", "dbo.Users");
            DropForeignKey("dbo.ClientProducts", "UsersId", "dbo.Users");
            DropForeignKey("dbo.ClientInfoes", "UsersId", "dbo.Users");
            DropIndex("dbo.SupportProducts", new[] { "UsersId" });
            DropIndex("dbo.ClientProducts", new[] { "UsersId" });
            DropIndex("dbo.ClientInfoes", new[] { "UsersId" });
            DropColumn("dbo.Users", "UserType");
            DropColumn("dbo.SupportProducts", "UsersId");
            DropColumn("dbo.ClientProducts", "UsersId");
            DropTable("dbo.ClientInfoes");
            CreateIndex("dbo.Users", "UserTypeId");
            CreateIndex("dbo.SupportProducts", "UserTypeId");
            CreateIndex("dbo.ClientProducts", "UserTypeId");
            AddForeignKey("dbo.Users", "UserTypeId", "dbo.UserTypes", "ID", cascadeDelete: true);
            AddForeignKey("dbo.SupportProducts", "UserTypeId", "dbo.UserTypes", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ClientProducts", "UserTypeId", "dbo.UserTypes", "ID", cascadeDelete: true);
        }
    }
}
