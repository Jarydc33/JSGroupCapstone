namespace SafestRouteApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixitplease : DbMigration
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
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AvoidanceRoutes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        RouteName = c.String(),
                        TopLeftLatitude = c.Single(nullable: false),
                        TopLeftLongitude = c.Single(nullable: false),
                        BottomRightLatitude = c.Single(nullable: false),
                        BottomRightLongitude = c.Single(nullable: false),
                        Reason = c.String(),
                        ObserveeId = c.Int(),
                        ObserverId = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Observees", t => t.ObserveeId)
                .ForeignKey("dbo.Observers", t => t.ObserverId)
                .Index(t => t.ObserveeId)
                .Index(t => t.ObserverId);
            
            CreateTable(
                "dbo.Observees",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                        ObserverId = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Observers", t => t.ObserverId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.ObserverId);
            
            CreateTable(
                "dbo.Observers",
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
            
            CreateTable(
                "dbo.LocationComments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        Comment = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.PhoneNumbers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        ObserverId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Observers", t => t.ObserverId, cascadeDelete: true)
                .Index(t => t.ObserverId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.SavedRoutes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        start_location = c.String(),
                        start_latitude = c.String(),
                        start_longitude = c.String(),
                        waypoint1 = c.String(),
                        waypoint2 = c.String(),
                        avoidstring = c.String(),
                        end_location = c.String(),
                        end_latitude = c.String(),
                        end_logitude = c.String(),
                        routeRequest = c.String(),
                        ObserveeId = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Observees", t => t.ObserveeId)
                .Index(t => t.ObserveeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SavedRoutes", "ObserveeId", "dbo.Observees");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.PhoneNumbers", "ObserverId", "dbo.Observers");
            DropForeignKey("dbo.LocationComments", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CustomSMS", "ObserverId", "dbo.Observers");
            DropForeignKey("dbo.AvoidanceRoutes", "ObserverId", "dbo.Observers");
            DropForeignKey("dbo.AvoidanceRoutes", "ObserveeId", "dbo.Observees");
            DropForeignKey("dbo.Observees", "ObserverId", "dbo.Observers");
            DropForeignKey("dbo.Observers", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Observees", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Administrators", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.SavedRoutes", new[] { "ObserveeId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.PhoneNumbers", new[] { "ObserverId" });
            DropIndex("dbo.LocationComments", new[] { "ApplicationUserId" });
            DropIndex("dbo.CustomSMS", new[] { "ObserverId" });
            DropIndex("dbo.Observers", new[] { "ApplicationUserId" });
            DropIndex("dbo.Observees", new[] { "ObserverId" });
            DropIndex("dbo.Observees", new[] { "ApplicationUserId" });
            DropIndex("dbo.AvoidanceRoutes", new[] { "ObserverId" });
            DropIndex("dbo.AvoidanceRoutes", new[] { "ObserveeId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Administrators", new[] { "ApplicationUserId" });
            DropTable("dbo.SavedRoutes");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.PhoneNumbers");
            DropTable("dbo.LocationComments");
            DropTable("dbo.CustomSMS");
            DropTable("dbo.Observers");
            DropTable("dbo.Observees");
            DropTable("dbo.AvoidanceRoutes");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Administrators");
        }
    }
}
