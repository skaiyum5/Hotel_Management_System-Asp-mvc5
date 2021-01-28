namespace HMS.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pictureEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccommodationPackagePictures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PictureID = c.Int(nullable: false),
                        AccommodationPackageID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Pictures", t => t.PictureID, cascadeDelete: true)
                .ForeignKey("dbo.AccommodationPackages", t => t.AccommodationPackageID, cascadeDelete: true)
                .Index(t => t.PictureID)
                .Index(t => t.AccommodationPackageID);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AccommodationPictures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PictureID = c.Int(nullable: false),
                        AccommodationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Pictures", t => t.PictureID, cascadeDelete: true)
                .ForeignKey("dbo.Accommodations", t => t.AccommodationID, cascadeDelete: true)
                .Index(t => t.PictureID)
                .Index(t => t.AccommodationID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccommodationPictures", "AccommodationID", "dbo.Accommodations");
            DropForeignKey("dbo.AccommodationPictures", "PictureID", "dbo.Pictures");
            DropForeignKey("dbo.AccommodationPackagePictures", "AccommodationPackageID", "dbo.AccommodationPackages");
            DropForeignKey("dbo.AccommodationPackagePictures", "PictureID", "dbo.Pictures");
            DropIndex("dbo.AccommodationPictures", new[] { "AccommodationID" });
            DropIndex("dbo.AccommodationPictures", new[] { "PictureID" });
            DropIndex("dbo.AccommodationPackagePictures", new[] { "AccommodationPackageID" });
            DropIndex("dbo.AccommodationPackagePictures", new[] { "PictureID" });
            DropTable("dbo.AccommodationPictures");
            DropTable("dbo.Pictures");
            DropTable("dbo.AccommodationPackagePictures");
        }
    }
}
