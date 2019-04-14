namespace SafestRouteApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCustomSMS : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomSMS",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        CustomMessage = c.String(),
                        ObserverId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Observers", t => t.ObserverId, cascadeDelete: true)
                .Index(t => t.ObserverId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomSMS", "ObserverId", "dbo.Observers");
            DropIndex("dbo.CustomSMS", new[] { "ObserverId" });
            DropTable("dbo.CustomSMS");
        }
    }
}
