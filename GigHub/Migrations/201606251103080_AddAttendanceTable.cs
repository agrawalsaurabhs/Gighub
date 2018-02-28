namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAttendanceTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendences",
                c => new
                    {
                        GigId = c.Int(nullable: false),
                        AttendeId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.GigId, t.AttendeId })
                .ForeignKey("dbo.AspNetUsers", t => t.AttendeId, cascadeDelete: true)
                .ForeignKey("dbo.Gigs", t => t.GigId)
                .Index(t => t.GigId)
                .Index(t => t.AttendeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attendences", "GigId", "dbo.Gigs");
            DropForeignKey("dbo.Attendences", "AttendeId", "dbo.AspNetUsers");
            DropIndex("dbo.Attendences", new[] { "AttendeId" });
            DropIndex("dbo.Attendences", new[] { "GigId" });
            DropTable("dbo.Attendences");
        }
    }
}
