using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SafestRouteApplication.Models;

namespace SafestRouteApplication.Controllers
{
    public class ObserversController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Observers
        public ActionResult Index()
        {
            var observers = db.Observers.Include(o => o.ApplicationUser);
            return View(observers.ToList());
        }

        // GET: Observers/Details/5
        public ActionResult Details(int? id)
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

        // GET: Observers/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Observers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,FirstName,LastName,ApplicationUserId")] Observer observer)
        {
            if (ModelState.IsValid)
            {
                db.Observers.Add(observer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", observer.ApplicationUserId);
            return View(observer);
        }

        // GET: Observers/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", observer.ApplicationUserId);
            return View(observer);
        }

        // POST: Observers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,FirstName,LastName,ApplicationUserId")] Observer observer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(observer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", observer.ApplicationUserId);
            return View(observer);
        }

        // GET: Observers/Delete/5
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
    }
}
