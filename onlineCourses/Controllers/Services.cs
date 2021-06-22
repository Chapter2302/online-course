using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using onlineCourses.Models;

namespace onlineCourses.Controllers
{
    public class Services
    {
        public static List<Course> relatedCourses = new List<Course>();
        public static HttpPostedFileBase registerPhoto;
        public static Account register = new Account(); 
        public static string hashString(string raw)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(raw));
                var sb = new StringBuilder(hash.Length);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}