namespace FirstProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateNullTokenModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tokens", "ComplainDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Tokens", "SolvedDate", c => c.DateTime(storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tokens", "SolvedDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Tokens", "ComplainDate", c => c.DateTime(nullable: false, storeType: "date"));
        }
    }
}
