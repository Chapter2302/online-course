using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using onlineCourses.Models;

namespace onlineCourses.Controllers
{
    public class DealsController : Controller
    {
        private onlineCoursesEntities db = new onlineCoursesEntities();

        // GET: Deals
        public ActionResult Index()
        {
            var deals = db.Deals.Include(d => d.Account).Include(d => d.Course);
            return View(deals.ToList());
        }

        // GET: Deals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            return View(deal);
        }

        // GET: Deals/Create
        public ActionResult Create()
        {
            ViewBag.StudentID = new SelectList(db.Accounts, "ID", "Username");
            ViewBag.CourseID = new SelectList(db.Courses, "ID", "Name");
            return View();
        }

        // POST: Deals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CourseID,StudentID,OrderDate,State,Review,Rating")] Deal deal)
        {
            if (ModelState.IsValid)
            {
                db.Deals.Add(deal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentID = new SelectList(db.Accounts, "ID", "Username", deal.StudentID);
            ViewBag.CourseID = new SelectList(db.Courses, "ID", "Name", deal.CourseID);
            return View(deal);
        }

        // GET: Deals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentID = new SelectList(db.Accounts, "ID", "Username", deal.StudentID);
            ViewBag.CourseID = new SelectList(db.Courses, "ID", "Name", deal.CourseID);
            return View(deal);
        }

        // POST: Deals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CourseID,StudentID,OrderDate,State,Review,Rating")] Deal deal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentID = new SelectList(db.Accounts, "ID", "Username", deal.StudentID);
            ViewBag.CourseID = new SelectList(db.Courses, "ID", "Name", deal.CourseID);
            return View(deal);
        }

        // GET: Deals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            return View(deal);
        }

        // POST: Deals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Deal deal = db.Deals.Find(id);
            db.Deals.Remove(deal);
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
