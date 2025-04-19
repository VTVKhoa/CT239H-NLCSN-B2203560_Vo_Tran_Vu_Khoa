namespace A23017_Cloud.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 128),
                        Password = c.String(nullable: false),
                        Point = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Username);
            
            CreateTable(
                "dbo.ScoreBoards",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Score = c.Int(nullable: false),
                        Username = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.Username)
                .Index(t => t.Username);
            
            CreateTable(
                "dbo.WoDs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Word = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScoreBoards", "Username", "dbo.Users");
            DropIndex("dbo.ScoreBoards", new[] { "Username" });
            DropTable("dbo.WoDs");
            DropTable("dbo.ScoreBoards");
            DropTable("dbo.Users");
        }
    }
}
