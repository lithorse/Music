namespace Music.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedalbum : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Album",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Song", "Album_Id", c => c.Int());
            CreateIndex("dbo.Song", "Album_Id");
            AddForeignKey("dbo.Song", "Album_Id", "dbo.Album", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Song", "Album_Id", "dbo.Album");
            DropIndex("dbo.Song", new[] { "Album_Id" });
            DropColumn("dbo.Song", "Album_Id");
            DropTable("dbo.Album");
        }
    }
}
