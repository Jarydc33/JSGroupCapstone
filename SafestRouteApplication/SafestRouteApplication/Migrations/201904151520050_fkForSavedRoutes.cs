namespace SafestRouteApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fkForSavedRoutes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SavedRoutes", "ObserveeId", c => c.Int());
            CreateIndex("dbo.SavedRoutes", "ObserveeId");
            AddForeignKey("dbo.SavedRoutes", "ObserveeId", "dbo.Observees", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SavedRoutes", "ObserveeId", "dbo.Observees");
            DropIndex("dbo.SavedRoutes", new[] { "ObserveeId" });
            DropColumn("dbo.SavedRoutes", "ObserveeId");
        }
    }
}
