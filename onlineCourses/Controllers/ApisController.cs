using onlineCourses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace onlineCourses.Controllers
{
    public class ApisController : Controller
    {
        // GET: Apis
        private onlineCoursesEntities db = new onlineCoursesEntities();
        // GET: Courses
        public ActionResult Index()
        {
            return Content("");
        }

        [HttpGet]
        [ActionName("get-courses")]
        public ActionResult GetCourses()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return Json(db.Courses.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("post-deals")]
        public ActionResult PostDeals(List<Course> courseCart)
        {
            try
            {
                for (int i = 0; i < courseCart.Count(); i++)
                {
                    Deal deal = new Deal();
                    deal.CourseID = courseCart[i].ID;
                    deal.StudentID = Convert.ToInt32(Session["id"]);
                    deal.State = "Pending";
                    deal.OrderDate = DateTime.Now.Date;

                    db.Deals.Add(deal);
                    db.SaveChanges();
                };
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ActionName("update-deal")]
        public ActionResult UpdateDeal(int id, int rate)
        {
            Deal deal = db.Deals.Where(d => d.ID == id).Select(d => d).FirstOrDefault();
            deal.Rating = rate;
            db.SaveChanges();
            return Json("success", JsonRequestBehavior.AllowGet);
        }
    }
}