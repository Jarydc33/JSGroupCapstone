namespace SafestRouteApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableAvoidRtFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AvoidanceRoutes", "ObserveeId", "dbo.Observees");
            DropIndex("dbo.AvoidanceRoutes", new[] { "ObserveeId" });
            AlterColumn("dbo.AvoidanceRoutes", "ObserveeId", c => c.Int());
            AlterColumn("dbo.LocationComments", "Latitude", c => c.String());
            AlterColumn("dbo.LocationComments", "Longitude", c => c.String());
            CreateIndex("dbo.AvoidanceRoutes", "ObserveeId");
            AddForeignKey("dbo.AvoidanceRoutes", "ObserveeId", "dbo.Observees", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AvoidanceRoutes", "ObserveeId", "dbo.Observees");
            DropIndex("dbo.AvoidanceRoutes", new[] { "ObserveeId" });
            AlterColumn("dbo.LocationComments", "Longitude", c => c.Single());
            AlterColumn("dbo.LocationComments", "Latitude", c => c.Single());
            AlterColumn("dbo.AvoidanceRoutes", "ObserveeId", c => c.Int(nullable: false));
            CreateIndex("dbo.AvoidanceRoutes", "ObserveeId");
            AddForeignKey("dbo.AvoidanceRoutes", "ObserveeId", "dbo.Observees", "id", cascadeDelete: true);
        }
    }
}
