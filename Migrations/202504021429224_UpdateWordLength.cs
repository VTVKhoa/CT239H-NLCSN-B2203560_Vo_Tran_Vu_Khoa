namespace A23017_Cloud.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateWordLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WoDs", "Word", c => c.String(nullable: false, maxLength: 5));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WoDs", "Word", c => c.String(nullable: false));
        }
    }
}
