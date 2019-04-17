using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using SafestRouteApplication.Models;

namespace SafestRouteApplication.Controllers
{
    public class ObserveesController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            Observee user = db.Observees.Where(c => c.ApplicationUserId == currentUserId).FirstOrDefault();
            return View(user);
        }
       
        public ActionResult Details()
        {
            string userId = User.Identity.GetUserId();
            Observee observee = db.Observees.Where(o => o.ApplicationUserId == userId).FirstOrDefault();
            observee.ApplicationUser = db.Users.Find(userId);
            return View(observee);
        }

        public ActionResult Create()
        {
            Observee user = new Observee();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Observee userToAdd)
        {
            string currentUserId = User.Identity.GetUserId();
            userToAdd.ApplicationUserId = currentUserId;
            if (ModelState.IsValid)
            {
                db.Observees.Add(userToAdd);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit()
        {
            string userId = User.Identity.GetUserId();
            Observee observee = db.Observees.Where(o => o.ApplicationUserId == userId).FirstOrDefault();
            return View(observee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Observee observee)
        {
            string userId = User.Identity.GetUserId();
            Observee user = db.Observees.Where(o => o.ApplicationUserId == userId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                user.FirstName = observee.FirstName;
                user.LastName = observee.LastName;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(observee);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Observer observer = db.Observers.Find(id);
            if (observer == null)
            {
                return HttpNotFound();
            }
            return View(observer);
        }

        public ActionResult PanicButton(int? id)
        {
            Observee panicObservee = db.Observees.Find(id);
            Observer guardian = db.Observers.Where(o => o.id == panicObservee.ObserverId).FirstOrDefault();
            ViewBag.PanicMessage = panicObservee.FirstName +" " + panicObservee.LastName + " has pushed the Panic Alert button. Their location is: "; //Add Location
            try
            {
                var phoneNumbers = db.PhoneNumbers.Where(p => p.ObserverId == guardian.id).ToList();
                foreach (var number in phoneNumbers)
                {
                    SendAlert.Send(ViewBag.PanicMessage, number.Number);
                }
            }
            catch
            {
                return View();
            }
            
            
            return View();
        }

        public ActionResult LeaveComment()
        {
            LocationComment comment = new LocationComment();
            string userId = User.Identity.GetUserId();
            Observee observee = db.Observees.Where(o => o.ApplicationUserId == userId).FirstOrDefault();
            comment.ApplicationUserId = observee.ApplicationUserId;
            return View(comment);
        }

        [HttpPost]
        public ActionResult LeaveComment(LocationComment comments)
        {
            comments.ApplicationUserId = User.Identity.GetUserId();
            db.LocationComments.Add(comments);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ChangeObserver()
        {
            ViewBag.ObserverMessage = "Add/Change Observer";
            ViewBag.RemoveObserverMessage = "Remove Observer";
            return View();
        }

        [HttpPost]
        public ActionResult RemoveObserver(ApplicationUser model)
        {
            string userId = User.Identity.GetUserId();
            Observee observee = db.Observees.Where(o => o.ApplicationUserId == userId).FirstOrDefault();
            Observer observer = db.Observers.Where(o => o.ApplicationUser.UserName == model.UserName).FirstOrDefault();

            if(observer == null)
            {
                ViewBag.RemoveObserverMessage = "That Observer UserName is not associated with your account.";
                return View("ChangeObserver");
            }
            else
            {
                observee.ObserverId = null;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult ChangeObserver(ApplicationUser model)
        {
            var user = db.Users.Where(u => u.UserName == model.UserName).FirstOrDefault();

            if (user == null)
            {
                ViewBag.ObserverMessage = "That UserName does not exist in the database.";
                return View();
            }
            else
            {
                
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); 
                var roles = userManager.GetRoles(user.Id);
                if (roles[0].Equals("Observer"))
                {
                    string userId = User.Identity.GetUserId();
                    Observee observee = db.Observees.Where(o => o.ApplicationUserId == userId).FirstOrDefault();
                    Observer observer = db.Observers.Where(o => o.ApplicationUserId == user.Id).FirstOrDefault();
                    observee.ObserverId = observer.id;
                    db.SaveChanges();
                    ViewBag.ObserverMessage = "That user has been added as your Observer.";
                    return RedirectToAction("Index");
                }
                return View();
            }
            
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public string GetCrimeData(double startlat, double startlong, double stoplat, double stoplong)
        {
            double NWLong;
            double NWLat;
            double SELong;
            double SELat;
            AllCrime crimeFilter = new AllCrime();
            List<float> filtered = new List<float>();
            string avoid = "";
            string url = "https://data.cityofchicago.org/resource/6zsd-86xi.json";
            WebRequest requestObject = WebRequest.Create(url);
            requestObject.Method = "GET";
            HttpWebResponse responseObject = null;
            responseObject = (HttpWebResponse)requestObject.GetResponse();

            string urlResult = null;
            using (Stream stream = responseObject.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                urlResult = sr.ReadToEnd();
                sr.Close();
            }

            crimeFilter.Property1 = JsonConvert.DeserializeObject<Class1[]>(urlResult);
            if (startlat < stoplat && startlong < stoplong)
            {
                NWLat = stoplat;
                NWLong = startlong;
                SELat = startlat;
                SELong = stoplong;
            }
            else if (startlat > stoplat && startlong < stoplong)
            {
                NWLat = startlat;
                NWLong = startlong;
                SELat = stoplat;
                SELong = stoplong;
            }
            else if (startlat > stoplat && startlong > stoplong)
            {
                NWLat = startlat;
                NWLong = stoplong;
                SELat = stoplat;
                SELong = startlong;
            }
            else
            {
                NWLat = stoplat;
                NWLong = stoplong;
                SELat = startlat;
                SELong = startlong;
            }
            for (int i = 0; i < crimeFilter.Property1.Length; i++)
            {
                try
                {

                    double longProperty = crimeFilter.Property1[i].location.coordinates[0];
                    double latProperty = crimeFilter.Property1[i].location.coordinates[1];
                   
                    if (longProperty >= NWLong && longProperty <= SELong)
                    {
                        if (latProperty <= NWLat && latProperty >= SELat)
                        {
                            avoid += ((latProperty + .0004) + "," + (longProperty + .0004) + ";" + (latProperty - .0004) + "," + (longProperty - .0004) + "!");
                        }

                    }

                }
                catch
                {

                }
            }
            if(avoid.Length > 0)
            {
                avoid = avoid.Remove(avoid.Length - 1);
            }
            

            //return avoid;
            return avoid;
        }

        // POST: Observers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Observer observer = db.Observers.Find(id);
            db.Observers.Remove(observer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Navigate()
        {
            NavigationViewModel navData = new NavigationViewModel();
            string id = User.Identity.GetUserId();
            navData.routes = db.SavedRoutes.Where(r => r.Observee.ApplicationUserId == id).Select(x => new SelectListItem() { Value = x.name, Text = x.name }).ToList();
            
            return View(navData);
        }
        [HttpPost]
        public ActionResult Navigate(NavigationViewModel navData)
        {
            ShowRouteViewModel model = new ShowRouteViewModel();
            string id = User.Identity.GetUserId();
            if (navData.selectedRoute != null)
            {
                model.savedRoute = db.SavedRoutes.Where(r => r.Observee.ApplicationUserId == id && r.name == navData.selectedRoute).Select(e => e).FirstOrDefault();
                model.observee = db.Observees.Where(e => e.ApplicationUserId == id).Select(e => e).FirstOrDefault();
                model.avoid = GetCrimeData(Double.Parse(model.savedRoute.start_latitude), Double.Parse(model.savedRoute.start_longitude), Double.Parse(model.savedRoute.end_latitude), Double.Parse(model.savedRoute.end_logitude));
                var thisId = db.Observees.Where(e => e.ApplicationUserId == id).Select(e => e.id).FirstOrDefault();
                List<AvoidanceRoute> avoidMarks = db.AvoidanceRoutes.Where(e => e.ObserveeId == thisId || e.ObserveeId == null).ToList();
                List<string> avoidCoords = new List<string>();
                foreach (AvoidanceRoute x in avoidMarks)
                {
                    avoidCoords.Add(x.TopLeftLatitude + "," + x.TopLeftLongitude + ";" + x.BottomRightLatitude + "," + x.BottomRightLongitude);
                    if (model.avoid != "")
                    {
                        model.avoid += "!";
                    }
                    model.avoid += (x.TopLeftLatitude + "," + x.TopLeftLongitude + ";" + x.BottomRightLatitude + "," + x.BottomRightLongitude);
                }
                model.route = CreateRoute.Retrieve(model.savedRoute.routeRequest);
            }
            else if (navData.StartAddress != null && navData.EndAddress != null)
            {
                GeoCode geo = new GeoCode();
                string startcoord = geo.Retrieve(navData.StartAddress);
                string[] waypoint1 = startcoord.Split(',');
                string stopcoord = geo.Retrieve(navData.EndAddress);
                string[] waypoint2 = stopcoord.Split(',');
                model.observee = db.Observees.Where(e => e.ApplicationUserId == id).Select(e => e).FirstOrDefault();
                model.avoid = GetCrimeData(Double.Parse(waypoint1[0]), Double.Parse(waypoint1[1]), Double.Parse(waypoint2[0]), Double.Parse(waypoint2[1]));
                var thisId = db.Observees.Where(e => e.ApplicationUserId == id).Select(e => e.id).FirstOrDefault();
                List<AvoidanceRoute> avoidMarks = db.AvoidanceRoutes.Where(e => e.ObserveeId == thisId || e.ObserveeId == null).ToList();
                List<string> avoidCoords = new List<string>();
                foreach (AvoidanceRoute x in avoidMarks)
                {
                    avoidCoords.Add(x.TopLeftLatitude + "," + x.TopLeftLongitude + ";" + x.BottomRightLatitude + "," + x.BottomRightLongitude);
                    if (model.avoid != "")
                    {
                        model.avoid += "!";
                    }
                    model.avoid += (x.TopLeftLatitude + "," + x.TopLeftLongitude + ";" + x.BottomRightLatitude + "," + x.BottomRightLongitude);
                }
                model.route = CreateRoute.Retrieve(startcoord, stopcoord, model.avoid);
            }
            else
            {
               return View();
            }
            TempData["myModel"] = model;
            return View("ShowRoute", model);
        }
        public ActionResult SaveRoute()
        {
            ShowRouteViewModel routeData = TempData["myModel"] as ShowRouteViewModel;
            SavedRoute newRoute = new SavedRoute();
            newRoute.ObserveeId = routeData.observee.id;
            newRoute.start_latitude = routeData.route.waypoint[0].mappedPosition.latitude.ToString();
            newRoute.start_longitude = routeData.route.waypoint[0].mappedPosition.longitude.ToString();
            newRoute.end_latitude = routeData.route.waypoint[1].mappedPosition.latitude.ToString();
            newRoute.end_logitude = routeData.route.waypoint[1].mappedPosition.longitude.ToString();
            newRoute.waypoint1 = newRoute.start_latitude + "," + newRoute.start_longitude;
            newRoute.waypoint2 = newRoute.end_latitude + "," + newRoute.end_logitude;
            newRoute.avoidstring = routeData.avoid;
            newRoute.routeRequest = "https://route.api.here.com/routing/7.2/calculateroute.json?app_id=" + Keys.HEREAppID + "&app_code=" + Keys.HEREAppCode + "&waypoint0=" + newRoute.waypoint1 + "&waypoint1=" + newRoute.waypoint2 + "&mode=fastest;pedestrian;traffic:disabled&avoidareas=" + newRoute.avoidstring;
            TempData["myRoute"] = newRoute;
            return View(newRoute);
        }
        [HttpPost]
        public ActionResult SaveRoute(SavedRoute routeData)
        {
            SavedRoute newRoute = TempData["myRoute"] as SavedRoute;
            newRoute.name = routeData.name;
            db.SavedRoutes.Add(newRoute);
            db.SaveChanges();
            return View("Index");
        }
        public ActionResult TraverseRoute()
        {
            ShowRouteViewModel routeData = TempData["myModel"] as ShowRouteViewModel;
            string Message = routeData.observee.FirstName +" "+ routeData.observee.LastName + " has begun their route";
            List<string> phoneNumbers = db.PhoneNumbers.Where(e => e.ObserverId == routeData.observee.ObserverId).Select(e => e.Number).ToList();
            foreach(string x in phoneNumbers)
            {
                SendAlert.Send(Message, x);
            }
            return View(routeData);
        }
        [HttpPost]
        public ActionResult TraverseRoute(SavedRoute routeData)
        {
            SavedRoute newRoute = TempData["myRoute"] as SavedRoute;
            newRoute.name = routeData.name;
            db.SavedRoutes.Add(newRoute);
            db.SaveChanges();
            return View("Index");
        }
        public ActionResult RouteComplete()
        {
            string id = User.Identity.GetUserId();
            Observee observee = db.Observees.Where(e => e.ApplicationUserId == id).FirstOrDefault();
            string Message = observee.FirstName + " " + observee.LastName + " has completed their route safely!";
            List<string> phoneNumbers = db.PhoneNumbers.Where(e => e.ObserverId == observee.ObserverId).Select(e => e.Number).ToList();
            foreach (string x in phoneNumbers)
            {
                SendAlert.Send(Message, x);
            }

            return View("Index");
        }

    }
   


    public class AllCrime
    {
        public Class1[] Property1 { get; set; }
    }

    public class Class1
    {
        public string computed_region_43wa_7qmu { get; set; }
        public string computed_region_6mkv_f3dw { get; set; }
        public string computed_region_awaf_s7ux { get; set; }
        public string computed_region_bdys_3d7i { get; set; }
        public string computed_region_d3ds_rm58 { get; set; }
        public string computed_region_d9mm_jgwp { get; set; }
        public string computed_region_rpca_8um6 { get; set; }
        public string computed_region_vrxf_vc4k { get; set; }
        public bool arrest { get; set; }
        public string beat { get; set; }
        public string block { get; set; }
        public string case_number { get; set; }
        public string community_area { get; set; }
        public DateTime date { get; set; }
        public string description { get; set; }
        public string district { get; set; }
        public bool domestic { get; set; }
        public string fbi_code { get; set; }
        public string id { get; set; }
        public string iucr { get; set; }
        public string latitude { get; set; }
        public Location location { get; set; }
        public string location_description { get; set; }
        public string longitude { get; set; }
        public string primary_type { get; set; }
        public DateTime updated_on { get; set; }
        public string ward { get; set; }
        public string x_coordinate { get; set; }
        public string y_coordinate { get; set; }
        public string year { get; set; }
    }

    public class Location
    {
        public string type { get; set; }
        public float[] coordinates { get; set; }
    }

    
}
