using Newtonsoft.Json;
using onlineCourses.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace onlineCourses.Controllers
{
    public class AuthController : Controller
    {
        private onlineCoursesEntities db = new onlineCoursesEntities();
        // GET: Auth
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ActionName("post-login")]
        public ActionResult Login(string username, string pwd)
        {
            string hashedPwd = Services.hashString(pwd);
            Account user = db.Accounts.ToList().Where(e => e.Username == username && e.Password == hashedPwd).FirstOrDefault();
            if (user != null)
            {
                Session["username"] = username;
                Session["pwd"] = hashedPwd;
                Session["id"] = user.ID;
                Session.Timeout = 20;
                return Json("success", JsonRequestBehavior.AllowGet);
            }

            return Json("fail", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("register")]
        public ActionResult Register()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ActionName("post-register")]
        public ActionResult RegisterPost(HttpPostedFileBase photo)
        {
            try
            {
                Account newUser = new Account();
                newUser.Username = Request["username"];
                newUser.Password = Services.hashString(Request["pwd"]);
                newUser.Address = Request["address"];
                DateTime birthday = new DateTime(Convert.ToInt32(Request["year"]), Convert.ToInt32(Request["month"]), Convert.ToInt32(Request["day"]));
                newUser.Birthday = birthday;
                newUser.Address = Request["address"];
                newUser.BankID = Convert.ToInt32(Request["bankid"]);
                newUser.Region = Request["region"];
                newUser.Country = Request["country"];
                newUser.StartDate = DateTime.Now.Date;
                newUser.Bio = Request["bio"];
                newUser.Email = Request["email"];
                newUser.School = Request["school"];

                Services.register = newUser;
                Services.registerPhoto = photo;

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("chapterchapter2302@gmail.com");
                mail.To.Add(newUser.Email);
                mail.Subject = "Activate OnlineCourses!";

                mail.IsBodyHtml = true;
                string htmlBody = @"Thank you for register! Click this button to activate: <a href = " + @"'https://localhost:44394/auth/activate'" + @"> ACTIVATE </a>";

                mail.Body = htmlBody;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("chapterchapter2302@gmail.com", "23024444");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                return View("CheckEmail");
            }
            catch
            {
                ViewBag.register = "fail";
                return View("Register");
            }
        }

        [ActionName("activate")]
        public ActionResult Activate()
        {
            try
            {
                db.Accounts.Add(Services.register);
                db.SaveChanges();

                Account acc = db.Accounts.ToList().Where(u => u.Username == Services.register.Username && u.Password == Services.register.Password).FirstOrDefault();

                try
                { 
                    string path = Server.MapPath("~/Resources/images/avatars/");
                    Directory.CreateDirectory(path);
                    string fileName = acc.ID + "." + Services.registerPhoto.ContentType.Replace("image/", "");
                    Services.registerPhoto.SaveAs(path + fileName);
                    acc.Photo = "/Resources/images/avatars/" + fileName;
                }
                catch
                {
                    acc.Photo = "/Resources/images/avatars/default.jpg";
                }
                db.SaveChanges();

                Session["username"] = Services.register.Username;
                Session["pwd"] = Services.register.Password;
                Session["id"] = acc.ID;
                Session.Timeout = 20;
                return RedirectToAction("Index", "User");
            }
            catch
            {
                return View("Error");
            }
        }

        [ActionName("logout")]
        public ActionResult Logout()
        {
            Session.Remove("username");
            Session.Remove("pwd");
            Session.Remove("id");
            return RedirectToAction("Index", "User");
        }

        [ActionName("forget-password")]
        public ActionResult getPwd(string email)
        {
            if (db.Accounts.Count(u => u.Email == email) > 0)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }
    }
}