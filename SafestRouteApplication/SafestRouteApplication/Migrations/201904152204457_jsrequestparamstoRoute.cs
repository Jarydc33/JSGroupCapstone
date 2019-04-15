namespace SafestRouteApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jsrequestparamstoRoute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SavedRoutes", "waypoint1", c => c.String());
            AddColumn("dbo.SavedRoutes", "waypoint2", c => c.String());
            AddColumn("dbo.SavedRoutes", "avoidstring", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SavedRoutes", "avoidstring");
            DropColumn("dbo.SavedRoutes", "waypoint2");
            DropColumn("dbo.SavedRoutes", "waypoint1");
        }
    }
}
