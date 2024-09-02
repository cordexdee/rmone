namespace ITAnalyticsBL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanyField : DbMigration
    {
        public override void Up()
        {            
            AddColumn("dbo.AspNetUsers", "Disabled", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropTable("dbo.AspNetUsers");
        }
    }
}
