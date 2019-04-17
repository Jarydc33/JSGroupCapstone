namespace SafestRouteApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AvoidanceRoutes", "RouteName", c => c.String());
            AddColumn("dbo.AvoidanceRoutes", "ObserverId", c => c.Int());
            CreateIndex("dbo.AvoidanceRoutes", "ObserverId");
            AddForeignKey("dbo.AvoidanceRoutes", "ObserverId", "dbo.Observers", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AvoidanceRoutes", "ObserverId", "dbo.Observers");
            DropIndex("dbo.AvoidanceRoutes", new[] { "ObserverId" });
            DropColumn("dbo.AvoidanceRoutes", "ObserverId");
            DropColumn("dbo.AvoidanceRoutes", "RouteName");
        }
    }
}
