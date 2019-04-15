using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SafestRouteApplication.Models;

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
            string userId = User.Identity.GetUserId();
            Observer observer = db.Observers.Where(o => o.ApplicationUserId == userId).FirstOrDefault();
            var phoneNumbers = db.PhoneNumbers.Where(p => p.ObserverId == observer.id).ToList();
            ViewBag.PhoneNumbers = phoneNumbers;
            return View(observer);
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
            ViewBag.RemovalMessage = "Phone Number Removal";
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
