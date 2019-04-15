namespace SafestRouteApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedRouteName : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SavedRoutes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        start_location = c.String(),
                        start_latitude = c.String(),
                        start_longitude = c.String(),
                        end_location = c.String(),
                        end_latitude = c.String(),
                        end_logitude = c.String(),
                        routeRequest = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.AvoidanceRoutes", "TopLeftLatitude", c => c.Single(nullable: false));
            AddColumn("dbo.AvoidanceRoutes", "TopLeftLongitude", c => c.Single(nullable: false));
            AddColumn("dbo.AvoidanceRoutes", "BottomRightLatitude", c => c.Single(nullable: false));
            AddColumn("dbo.AvoidanceRoutes", "BottomRightLongitude", c => c.Single(nullable: false));
            DropColumn("dbo.AvoidanceRoutes", "Latitude");
            DropColumn("dbo.AvoidanceRoutes", "Longitude");
            DropTable("dbo.Routes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.AvoidanceRoutes", "Longitude", c => c.Single(nullable: false));
            AddColumn("dbo.AvoidanceRoutes", "Latitude", c => c.Single(nullable: false));
            DropColumn("dbo.AvoidanceRoutes", "BottomRightLongitude");
            DropColumn("dbo.AvoidanceRoutes", "BottomRightLatitude");
            DropColumn("dbo.AvoidanceRoutes", "TopLeftLongitude");
            DropColumn("dbo.AvoidanceRoutes", "TopLeftLatitude");
            DropTable("dbo.SavedRoutes");
        }
    }
}
