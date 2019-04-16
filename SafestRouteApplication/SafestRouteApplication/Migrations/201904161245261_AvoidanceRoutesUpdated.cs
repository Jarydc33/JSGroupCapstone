namespace SafestRouteApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AvoidanceRoutesUpdated : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AvoidanceRoutes", "ObserveeId", "dbo.Observees");
            DropIndex("dbo.AvoidanceRoutes", new[] { "ObserveeId" });
            AddColumn("dbo.SavedRoutes", "waypoint1", c => c.String());
            AddColumn("dbo.SavedRoutes", "waypoint2", c => c.String());
            AddColumn("dbo.SavedRoutes", "avoidstring", c => c.String());
            AlterColumn("dbo.AvoidanceRoutes", "ObserveeId", c => c.Int());
            CreateIndex("dbo.AvoidanceRoutes", "ObserveeId");
            AddForeignKey("dbo.AvoidanceRoutes", "ObserveeId", "dbo.Observees", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AvoidanceRoutes", "ObserveeId", "dbo.Observees");
            DropIndex("dbo.AvoidanceRoutes", new[] { "ObserveeId" });
            AlterColumn("dbo.AvoidanceRoutes", "ObserveeId", c => c.Int(nullable: false));
            DropColumn("dbo.SavedRoutes", "avoidstring");
            DropColumn("dbo.SavedRoutes", "waypoint2");
            DropColumn("dbo.SavedRoutes", "waypoint1");
            CreateIndex("dbo.AvoidanceRoutes", "ObserveeId");
            AddForeignKey("dbo.AvoidanceRoutes", "ObserveeId", "dbo.Observees", "id", cascadeDelete: true);
        }
    }
}
