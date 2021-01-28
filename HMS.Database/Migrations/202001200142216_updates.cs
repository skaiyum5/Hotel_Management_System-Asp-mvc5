namespace HMS.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updates : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AccommodationPackages", "AccommodationType_ID", "dbo.AccommodationTypes");
            DropIndex("dbo.AccommodationPackages", new[] { "AccommodationType_ID" });
            RenameColumn(table: "dbo.AccommodationPackages", name: "AccommodationType_ID", newName: "AccommodationTypeID");
            AlterColumn("dbo.AccommodationPackages", "AccommodationTypeID", c => c.Int(nullable: false));
            CreateIndex("dbo.AccommodationPackages", "AccommodationTypeID");
            AddForeignKey("dbo.AccommodationPackages", "AccommodationTypeID", "dbo.AccommodationTypes", "ID", cascadeDelete: true);
            DropColumn("dbo.AccommodationPackages", "AccommodationID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccommodationPackages", "AccommodationID", c => c.Int(nullable: false));
            DropForeignKey("dbo.AccommodationPackages", "AccommodationTypeID", "dbo.AccommodationTypes");
            DropIndex("dbo.AccommodationPackages", new[] { "AccommodationTypeID" });
            AlterColumn("dbo.AccommodationPackages", "AccommodationTypeID", c => c.Int());
            RenameColumn(table: "dbo.AccommodationPackages", name: "AccommodationTypeID", newName: "AccommodationType_ID");
            CreateIndex("dbo.AccommodationPackages", "AccommodationType_ID");
            AddForeignKey("dbo.AccommodationPackages", "AccommodationType_ID", "dbo.AccommodationTypes", "ID");
        }
    }
}
