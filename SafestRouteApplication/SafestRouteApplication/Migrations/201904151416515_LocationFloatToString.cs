namespace SafestRouteApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocationFloatToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LocationComments", "Latitude", c => c.String());
            AlterColumn("dbo.LocationComments", "Longitude", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LocationComments", "Longitude", c => c.Single());
            AlterColumn("dbo.LocationComments", "Latitude", c => c.Single());
        }
    }
}
