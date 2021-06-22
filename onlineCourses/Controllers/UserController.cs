using Newtonsoft.Json;
using onlineCourses.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace onlineCourses.Controllers
{
    public class UserController : Controller
    {
        private onlineCoursesEntities db = new onlineCoursesEntities();
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        [HttpGet]
        [ActionName("my-account")]
        public ActionResult GetProfile()
        {
            Account user = new Account();
            user = db.Accounts.ToList().Where(u => u.Username == Session["username"].ToString() && u.Password == Session["pwd"].ToString()).FirstOrDefault();
            ViewBag.user = user;
            return View("Profile");
        }

        [HttpPost]
        [ActionName("update-profile")]
        public ActionResult Update(HttpPostedFileBase photo)
        {
            Account user = new Account();
            user = db.Accounts.Find(Convert.ToInt32(Request["id"]));

            user.Username = Request["username"];
            user.Password = Services.hashString(Request["pwd"]);
            user.Address = Request["address"];
            try
            {
                DateTime birthday = new DateTime(Convert.ToInt32(Request["year"]), Convert.ToInt32(Request["month"]), Convert.ToInt32(Request["day"]));
                user.Birthday = birthday;
            }
            catch
            { }
            user.Address = Request["address"];
            user.BankID = Convert.ToInt32(Request["bankid"]);
            user.Region = Request["region"];
            user.Country = Request["country"];
            user.StartDate = DateTime.Now.Date;
            user.Bio = Request["bio"];
            user.Email = Request["email"];
            user.School = Request["school"];

            if (photo != null)
            {
                string path = Server.MapPath("~/Resources/images/avatars/");
                Directory.CreateDirectory(path);
                try
                { 
                    string fileName = user.ID + "." + photo.ContentType.Replace("image/", "");
                    photo.SaveAs(path + fileName);
                    user.Photo = "/Resources/images/avatars/" + fileName;
                }
                catch
                {
                    user.Photo = "/Resources/images/avatars/default.jpg";
                }
            }

            db.SaveChanges();
            return RedirectToAction("my-account", "User");
        }

        [ActionName("course-detail")]
        public ActionResult GetCourseDetail(int id)
        {
            Course course = new Course();
            course = db.Courses.Find(id);
            Account teacher = new Account();
            teacher = db.Accounts.Find(course.TeacherID);
            ViewBag.course = course;
            ViewBag.teacher = teacher.Username;
            
            if (Services.relatedCourses.Exists(c => c.ID == course.ID) == false)
            {
                if (Services.relatedCourses.Count() > 4)
                    Services.relatedCourses.RemoveAt(0);
                Services.relatedCourses.Add(course);
            }
            ViewBag.relatedCourses = Services.relatedCourses;
            return View("GetCourseDetail");
        }

        [HttpGet]
        [ActionName("get-courses-list")]
        public ActionResult GetCourseList()
        {
            int count = db.Courses.Count();
            int pages = count / 10;
            if (count % 10 > 0)
            {
                pages = count/10 + 1;
            }
            
            List<Course> courseList = new List<Course>();
            courseList = db.Courses.Take(9).ToList();
            
            ViewBag.courses = courseList;
            ViewBag.pages = pages;
            return View("GetCourseList");
        }

        [HttpGet]
        [ActionName("view-cart")]
        public ActionResult CheckOut()
        {
            return View("ViewCart");
        }

        [HttpGet]
        [ActionName("my-courses")]
        public ActionResult MyCourses()
        {
            List<Course> myCourses = new List<Course>();
            List<Deal> deals = new List<Deal>();
            int id = Convert.ToInt32(Session["id"].ToString());
            deals = db.Deals.Where(d => d.StudentID == id).ToList();
            for(int i=0; i<deals.Count; i++)
            {
                if (deals[i].State != "Failed")
                {
                    Course course = new Course();
                    course = db.Courses.Find(deals[i].CourseID);
                    myCourses.Add(course);
                }
            }
            ViewBag.courses = myCourses;
            ViewBag.deals = deals;
            return View("MyCourses");
        }
    }
}
