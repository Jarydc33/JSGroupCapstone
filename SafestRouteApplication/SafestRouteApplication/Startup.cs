using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using SafestRouteApplication.Models;

[assembly: OwinStartupAttribute(typeof(SafestRouteApplication.Startup))]
namespace SafestRouteApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesandUsers();
        }

        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
  
            if (!roleManager.RoleExists("Admin"))
            {
 
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "Admin";
                user.Email = "Admin@SafestRoute.com";

                string userPWD = "P@$$w0rd!!";

                var chkUser = UserManager.Create(user, userPWD);
  
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }
   
            if (!roleManager.RoleExists("Observer"))
            {
                var role = new IdentityRole();
                role.Name = "Observer";
                roleManager.Create(role);

            }
  
            if (!roleManager.RoleExists("Observee"))
            {
                var role = new IdentityRole();
                role.Name = "Observee";
                roleManager.Create(role);

            }
        }

    }
}
