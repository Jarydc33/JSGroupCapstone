using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
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
       
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Observee observee = db.Observees.Find(id);
            observee.ApplicationUser = db.Users.Where(u => u.Id == observee.ApplicationUserId).FirstOrDefault();
            try
            {
                observee.Observer = db.Observers.Where(o => o.id == observee.ObserverId).FirstOrDefault();
                observee.Observer.ApplicationUser = db.Users.Where(u => u.Id == observee.Observer.ApplicationUserId).FirstOrDefault();
            }
            catch
            {
                observee.Observer = null;
            }
            
            if (observee == null)
            {
                return HttpNotFound();
            }
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

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Observee observee = db.Observees.Find(id);
            //observee.Observer = db.Observers.Where(o => o.id == observee.ObserverId).FirstOrDefault();
            if (observee == null)
            {
                return HttpNotFound();
            }
            
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
            var phoneNumbers = db.PhoneNumbers.Where(p => p.ObserverId == guardian.id).ToList();
            foreach (var number in phoneNumbers)
            {
                SendAlert.Send(ViewBag.PanicMessage, number.Number);
            }
            
            return View();
        }

        public ActionResult LeaveComment(int? id)
        {
            LocationComment comment = new LocationComment();
            return View(comment);
        }

        [HttpPost]
        public ActionResult LeaveComment(LocationComment comments, string Address)
        {
            var location = GeoCode.Retrieve(Address);
            string[] coordinates = location.Split(',');
            comments.ApplicationUserId = User.Identity.GetUserId();
            comments.Latitude = coordinates[0];
            comments.Longitude = coordinates[1];
            db.LocationComments.Add(comments);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ChangeObserver()
        {
            ViewBag.ObserverMessage = "Change Observer";
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

        public List<float> GetCrimeData(float NELat, float NELong, float SWLat, float SWLong)
        {
            AllCrime crimeFilter = new AllCrime();
            List<float> filtered = new List<float>();
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

            //float NWLong = NELong; dont need
            //float NWLat = SWLat;
            //float SELong = SWLong;
            //float SELat = NELat;

            crimeFilter.Property1 = JsonConvert.DeserializeObject<Class1[]>(urlResult);

            for (int i = 0; i < crimeFilter.Property1.Length; i++)
            {
                try
                {
                    float longProperty = crimeFilter.Property1[i].location.coordinates[0];
                    float latProperty = crimeFilter.Property1[i].location.coordinates[1];
                    if (longProperty <= NELong && longProperty >= SWLong)
                    {
                        if (latProperty <= NELat && latProperty >= SWLat)
                        {
                            filtered.Add(crimeFilter.Property1[i].location.coordinates[0]);
                            filtered.Add(crimeFilter.Property1[i].location.coordinates[1]);
                        }

                    }
                }
                catch
                {

                }
            }

            return filtered;
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult Navigate()
        {
            NavigationViewModel navData = new NavigationViewModel();
            //navData.routes = db.SavedRoutes.Where(r => r.name)

            return View(navData);
        }
        //[HttpPost]
        //public ActionResult Navigate()
        //{


        //    return View();
        //}
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
