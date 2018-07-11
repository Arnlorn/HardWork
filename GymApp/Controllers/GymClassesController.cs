using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GymApp.Models;

namespace GymApp.Controllers
{
    public class GymClassesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: GymClasses
        public ActionResult Index()
        {
            ViewBag.Title = "Index";
            return View(db.GymClasses.ToList());
        }

        public ActionResult FilteredIndex(string filter)
        {
            var currentUser = db.Users.Single(u => u.UserName == User.Identity.Name);
            if (filter == "HistoricPasses")
            {
                var historicGymClasses = db.GymClasses.Where(p => p.StartTime < DateTime.Now );
                var historicGymClassesForCurrentUser = new List<GymClass>();
                foreach (var gymClass in historicGymClasses)
                {
                    if (gymClass.AttendingMembers.Contains(currentUser)) { historicGymClassesForCurrentUser.Add(gymClass);}
                }
                ViewBag.Title = "Historic Gym Passes";
                return View("Index",historicGymClassesForCurrentUser);
            }
            else if (filter == "BookedPasses")
            {
                var futureGymClasses = db.GymClasses.Where(p => p.StartTime > DateTime.Now);
                var futureGymClassesForCurrentUser = new List<GymClass>();
                foreach (var gymClass in futureGymClasses)
                {
                    if (gymClass.AttendingMembers.Contains(currentUser)) { futureGymClassesForCurrentUser.Add(gymClass); }
                }
                ViewBag.Title = "Booked Passes";
                return View("Index", futureGymClassesForCurrentUser);
            }
            else
            {
                ViewBag.Title = "Index";
                return View("Index", db.GymClasses.ToList());
            }







        }

 
        // GET: GymClasses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GymClass gymClass = db.GymClasses.Find(id);
            if (gymClass == null)
            {
                return HttpNotFound();
            }
            return View(gymClass);
        }

        // GET: GymClasses/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                db.GymClasses.Add(gymClass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gymClass);
        }

        // GET: GymClasses/Edit/5

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GymClass gymClass = db.GymClasses.Find(id);
            if (gymClass == null)
            {
                return HttpNotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gymClass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gymClass);
        }

        // GET: GymClasses/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GymClass gymClass = db.GymClasses.Find(id);
            if (gymClass == null)
            {
                return HttpNotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            GymClass gymClass = db.GymClasses.Find(id);
            db.GymClasses.Remove(gymClass);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult BookingToggle(int id)
        {
            var currentClass = db.GymClasses.Single(g => g.Id == id);
            var currentUser = db.Users.Single(u => u.UserName == User.Identity.Name);
            
            if (currentClass.AttendingMembers.Contains(currentUser))
            {
                currentClass.AttendingMembers.Remove(currentUser);
                db.SaveChanges();
            }
            else
            {
                currentClass.AttendingMembers.Add(currentUser);
                db.SaveChanges();
            }
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
