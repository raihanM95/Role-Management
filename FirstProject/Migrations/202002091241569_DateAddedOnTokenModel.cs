namespace FirstProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateAddedOnTokenModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tokens", "ComplainDate", c => c.DateTime(nullable: false, storeType: "date"));
            AddColumn("dbo.Tokens", "SolvedDate", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tokens", "SolvedDate");
            DropColumn("dbo.Tokens", "ComplainDate");
        }
    }
}
