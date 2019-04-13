namespace SafestRouteApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Observees", "ObserverId", "dbo.Observers");
            DropIndex("dbo.Observees", new[] { "ObserverId" });
            AlterColumn("dbo.Observees", "ObserverId", c => c.Int());
            CreateIndex("dbo.Observees", "ObserverId");
            AddForeignKey("dbo.Observees", "ObserverId", "dbo.Observers", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Observees", "ObserverId", "dbo.Observers");
            DropIndex("dbo.Observees", new[] { "ObserverId" });
            AlterColumn("dbo.Observees", "ObserverId", c => c.Int(nullable: false));
            CreateIndex("dbo.Observees", "ObserverId");
            AddForeignKey("dbo.Observees", "ObserverId", "dbo.Observers", "id", cascadeDelete: true);
        }
    }
}
