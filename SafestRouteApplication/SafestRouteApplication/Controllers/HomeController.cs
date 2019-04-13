using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SafestRouteApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SafestRouteApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;

                if (isAdminUser())
                {
                    ViewBag.displayMenu = "Admin";
                    return RedirectToAction("Index", "Administrators");
                }
                else if (isObserverUser())
                {
                    ViewBag.displayMenu = "Observer";
                    return RedirectToAction("Index", "Observers");
                }
                else if (isObserveeUser())
                {
                    ViewBag.displayMenu = "Observee";
                    return RedirectToAction("Index", "Observees");
                }
            }
            return View();
        }

        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                var getRole = UserManager.GetRoles(user.GetUserId());
                if (getRole[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public Boolean isObserverUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                var getRole = UserManager.GetRoles(user.GetUserId());
                if (getRole[0].ToString() == "Observer")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public Boolean isObserveeUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext db = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                var getRole = UserManager.GetRoles(user.GetUserId());
                if (getRole[0].ToString() == "Observee")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}