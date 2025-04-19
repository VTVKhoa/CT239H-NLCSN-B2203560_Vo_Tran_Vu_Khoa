namespace A23017_Cloud.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeWoD : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WoDs", "Phonetic", c => c.String(nullable: false));
            AddColumn("dbo.WoDs", "Definition", c => c.String(nullable: false));
            AlterColumn("dbo.WoDs", "Word", c => c.String(nullable: false));
            DropColumn("dbo.WoDs", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WoDs", "Date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.WoDs", "Word", c => c.String());
            DropColumn("dbo.WoDs", "Definition");
            DropColumn("dbo.WoDs", "Phonetic");
        }
    }
}
