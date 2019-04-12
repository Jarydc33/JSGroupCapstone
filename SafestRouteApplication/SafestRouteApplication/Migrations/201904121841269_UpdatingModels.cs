namespace SafestRouteApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatingModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AvoidanceRoutes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Latitude = c.Single(nullable: false),
                        Longitude = c.Single(nullable: false),
                        Reason = c.String(),
                        ObserveeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Observees", t => t.ObserveeId, cascadeDelete: true)
                .Index(t => t.ObserveeId);
            
            CreateTable(
                "dbo.LocationComments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Latitude = c.Single(),
                        Longitude = c.Single(),
                        Comment = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.PhoneNumbers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        ObserverId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Observers", t => t.ObserverId, cascadeDelete: true)
                .Index(t => t.ObserverId);
            
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.Observees", "ObserverId", c => c.Int(nullable: false));
            CreateIndex("dbo.Observees", "ObserverId");
            AddForeignKey("dbo.Observees", "ObserverId", "dbo.Observers", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PhoneNumbers", "ObserverId", "dbo.Observers");
            DropForeignKey("dbo.LocationComments", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AvoidanceRoutes", "ObserveeId", "dbo.Observees");
            DropForeignKey("dbo.Observees", "ObserverId", "dbo.Observers");
            DropIndex("dbo.PhoneNumbers", new[] { "ObserverId" });
            DropIndex("dbo.LocationComments", new[] { "ApplicationUserId" });
            DropIndex("dbo.Observees", new[] { "ObserverId" });
            DropIndex("dbo.AvoidanceRoutes", new[] { "ObserveeId" });
            DropColumn("dbo.Observees", "ObserverId");
            DropTable("dbo.Routes");
            DropTable("dbo.PhoneNumbers");
            DropTable("dbo.LocationComments");
            DropTable("dbo.AvoidanceRoutes");
        }
    }
}
