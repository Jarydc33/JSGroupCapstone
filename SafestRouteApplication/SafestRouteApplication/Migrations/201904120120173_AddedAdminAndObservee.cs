namespace SafestRouteApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAdminAndObservee : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrators",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Observees",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Observees", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Administrators", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Observees", new[] { "ApplicationUserId" });
            DropIndex("dbo.Administrators", new[] { "ApplicationUserId" });
            DropTable("dbo.Observees");
            DropTable("dbo.Administrators");
        }
    }
}
