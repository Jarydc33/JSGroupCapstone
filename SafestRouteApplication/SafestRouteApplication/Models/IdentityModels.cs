﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SafestRouteApplication.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Observer> Observers { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Observee> Observees { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<AvoidanceRoute> AvoidanceRoutes { get; set; }
        public DbSet<SavedRoute> Routes { get; set; }
        public DbSet<LocationComment> LocationComments { get; set; }
        public DbSet<CustomSMS> CustomSMSs { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public System.Data.Entity.DbSet<SafestRouteApplication.Models.Observer> Observers { get; set; }

        //public System.Data.Entity.DbSet<SafestRouteApplication.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}