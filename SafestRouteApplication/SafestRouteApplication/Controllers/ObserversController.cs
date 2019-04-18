using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using SafestRouteApplication.Models;
using SafestRouteApplication;

namespace SafestRouteApplication.Controllers
{
    public class ObserversController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var observer = db.Observers.Where(o => o.ApplicationUserId == userId).FirstOrDefault();
            var observees = db.Observees.Where(o => o.ObserverId == observer.id).ToList();
            return View(observees);
        }
        
        public ActionResult Details()
        {
            ObserverDetailsViewModel details = new ObserverDetailsViewModel();
            string userId = User.Identity.GetUserId();
            Observer observer = db.Observers.Where(o => o.ApplicationUserId == userId).FirstOrDefault();
            var phoneNumbers = db.PhoneNumbers.Where(p => p.ObserverId == observer.id).ToList();
            details.FirstName = observer.FirstName;
            details.LastName = observer.LastName;
            details.Numbers = phoneNumbers;
            return View(details);
        }

        public ActionResult Create()
        {
            Observer user = new Observer();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Observer observer)
        {
            string currentUserId = User.Identity.GetUserId();
            observer.ApplicationUserId = currentUserId;
            if (ModelState.IsValid)
            {
                db.Observers.Add(observer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(observer);
        }

        public ActionResult Edit()
        {
            string userId = User.Identity.GetUserId();
            Observer observer = db.Observers.Where(o => o.ApplicationUserId == userId).FirstOrDefault();
            return View(observer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Observer observer)
        {
            string userId = User.Identity.GetUserId();
            Observer observerToChange = db.Observers.Where(o => o.ApplicationUserId == userId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                observerToChange.FirstName = observer.FirstName;
                observerToChange.LastName = observer.LastName;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(observer);
        }

        public ActionResult AddCustomAvoidance()
        {
            AvoidanceRouteViewModel newRoute = new AvoidanceRouteViewModel();
            string userId = User.Identity.GetUserId();
            Observer observer = db.Observers.Where(o => o.ApplicationUserId == userId).FirstOrDefault();
            newRoute.RouteNames = new SelectList(db.AvoidanceRoutes.Where(a => a.ObserverId == observer.id).ToList(),"id","RouteName");
            newRoute.Observees = new SelectList(db.Observees.Where(o => o.ObserverId == observer.id).ToList(),"id","FirstName");
            return View(newRoute);
        }

        [HttpPost]
        public ActionResult AddCustomAvoidance(AvoidanceRouteViewModel newRoute)
        {
            AvoidanceRoute routeToAdd = new AvoidanceRoute();
            string userId = User.Identity.GetUserId();
            Observer observer = db.Observers.Where(o => o.ApplicationUserId == userId).FirstOrDefault();
            newRoute.id = int.Parse(newRoute.ObserveeId);
            Observee observee = db.Observees.Where(o => o.id == newRoute.id).FirstOrDefault();
            routeToAdd.BottomRightLatitude = newRoute.BottomRightLatitude;
            routeToAdd.BottomRightLongitude = newRoute.BottomRightLongitude;
            routeToAdd.TopLeftLatitude = newRoute.TopLeftLatitude;
            routeToAdd.TopLeftLongitude = newRoute.TopLeftLongitude;
            routeToAdd.Reason = newRoute.Reason;
            routeToAdd.RouteName = newRoute.Name;
            routeToAdd.ObserveeId = observee.id;
            routeToAdd.ObserverId = observer.id;
            db.AvoidanceRoutes.Add(routeToAdd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RemoveAvoidanceRoute(AvoidanceRouteViewModel routeToRemove)
        {
            AvoidanceRoute removal = db.AvoidanceRoutes.Find(routeToRemove.id);
            db.AvoidanceRoutes.Remove(removal);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ViewAvoidanceRoute(AvoidanceRouteViewModel routeToFind)
        {
            string userId = User.Identity.GetUserId();
            Observer observer = db.Observers.Where(o => o.ApplicationUserId == userId).FirstOrDefault();
            routeToFind.RouteNames = new SelectList(db.AvoidanceRoutes.Where(r => r.ObserverId == observer.id).ToList(), "id", "RouteName");
            if (routeToFind.id == null)
            {
                return View(routeToFind);
            }
            AvoidanceRoute foundRoute = db.AvoidanceRoutes.Where(r => r.id == routeToFind.id).FirstOrDefault();
            routeToFind.Reason = foundRoute.Reason;
            routeToFind.BottomRightLatitude = foundRoute.BottomRightLatitude;
            routeToFind.BottomRightLongitude = foundRoute.BottomRightLongitude;
            routeToFind.TopLeftLatitude = foundRoute.TopLeftLatitude;
            routeToFind.TopLeftLongitude = foundRoute.TopLeftLongitude;
            return View(routeToFind);
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Observer observer = db.Observers.Find(id);
            db.Observers.Remove(observer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddPhoneNumber()
        {
            ViewBag.RemovalMessage = "Remove a Number";
            PhoneNumber newNumber = new PhoneNumber();
            return View(newNumber);
        }

        [HttpPost]
        public ActionResult AddPhoneNumber(PhoneNumber numberToAdd)
        {
            string userId = User.Identity.GetUserId();
            Observer user = db.Observers.Where(o => o.ApplicationUserId == userId).FirstOrDefault();
            numberToAdd.ObserverId = user.id;
            db.PhoneNumbers.Add(numberToAdd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemovePhoneNumber(PhoneNumber numberToRemove)
        {
            PhoneNumber removal = db.PhoneNumbers.Where(p => p.Number == numberToRemove.Number).FirstOrDefault();
            if(removal == null)
            {
                ViewBag.RemovalMessage = "That phone number was not on your account";
                return View("AddPhoneNumber");
            }
            else
            {
                db.PhoneNumbers.Remove(removal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
        }
        public ActionResult ViewSavedRoutes()
        {
            string id = User.Identity.GetUserId();
            int observerid = db.Observers.Where(e => e.ApplicationUserId == id).Select(e => e.id).FirstOrDefault();
            List<Observee> observees = db.Observees.Where(e => e.ObserverId == observerid).ToList();
            List<SavedRoute> routes = new List<SavedRoute>();
            List<SavedRoute> tempRoutes = new List<SavedRoute>();
            foreach (Observee x in observees)
            {
                tempRoutes = db.SavedRoutes.Where(e => e.ObserveeId == x.id).ToList();
                foreach(SavedRoute y in tempRoutes)
                {
                    routes.Add(y);
                }
            }
            return View(routes);
        }
        public ActionResult ViewRoute(int id)
        {
            SavedRoute route = db.SavedRoutes.Where(e => e.id == id).FirstOrDefault();
            return View(route);
        }
        public ActionResult CreateRoutes()
        {
            ObserverSaveRouteModel navData = new ObserverSaveRouteModel();
            string id = User.Identity.GetUserId();
            int observerid = db.Observers.Where(e => e.ApplicationUserId == id).Select(e => e.id).FirstOrDefault();
            navData.observees = db.Observees.Where(e => e.ObserverId == observerid).Select(e=> new SelectListItem() { Value = e.id+"", Text = (e.FirstName+" "+e.LastName) }).ToList();
            return View(navData);
        }
        [HttpPost]
        public ActionResult CreateRoutes(ObserverSaveRouteModel navData)
        {
            ShowRouteViewModel model = new ShowRouteViewModel();
            string id = User.Identity.GetUserId();
            if (navData.nav.StartAddress != null && navData.nav.EndAddress != null)
            {
                GeoCode geo = new GeoCode();
                string startcoord = geo.Retrieve(navData.nav.StartAddress);
                string[] waypoint1 = startcoord.Split(',');
                string stopcoord = geo.Retrieve(navData.nav.EndAddress);
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

                try
                {
                    SavedRoute newRoute = new SavedRoute();
                    newRoute.name = navData.route.name;
                    newRoute.ObserveeId = int.Parse(navData.observeeId);
                    newRoute.start_latitude = model.route.waypoint[0].mappedPosition.latitude.ToString();
                    newRoute.start_longitude = model.route.waypoint[0].mappedPosition.longitude.ToString();
                    newRoute.end_latitude = model.route.waypoint[1].mappedPosition.latitude.ToString();
                    newRoute.end_logitude = model.route.waypoint[1].mappedPosition.longitude.ToString();
                    newRoute.waypoint1 = newRoute.start_latitude + "," + newRoute.start_longitude;
                    newRoute.waypoint2 = newRoute.end_latitude + "," + newRoute.end_logitude;
                    newRoute.avoidstring = model.avoid;
                    newRoute.routeRequest = "https://route.api.here.com/routing/7.2/calculateroute.json?app_id=" + Keys.HEREAppID + "&app_code=" + Keys.HEREAppCode + "&waypoint0=" + newRoute.waypoint1 + "&waypoint1=" + newRoute.waypoint2 + "&mode=fastest;pedestrian;traffic:disabled&avoidareas=" + newRoute.avoidstring;

                    db.SavedRoutes.Add(newRoute);
                    db.SaveChanges();
                }
                catch
                {

                }
            }
            else
            {
                return View();
            }
            
            
            return RedirectToAction("Index");
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
            if (avoid.Length > 0)
            {
                avoid = avoid.Remove(avoid.Length - 1);
            }


            //return avoid;
            return avoid;
        }








        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
