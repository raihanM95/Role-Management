namespace FirstProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TokenModel : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Tockens", newName: "Tokens");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Tokens", newName: "Tockens");
        }
    }
}
