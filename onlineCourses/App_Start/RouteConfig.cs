using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace onlineCourses
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Api",
                url: "api/{action}/{id}",
                defaults: new { controller = "Apis", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "User",
                url: "user/{action}/{id}",
                defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Authentication",
                url: "auth/{action}/{id}",
                defaults: new { controller = "Auth", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AdminAccounts",
                url: "admin/accounts/{action}/{id}",
                defaults: new { controller = "Accounts", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AdminCourses",
                url: "admin/courses/{action}/{id}",
                defaults: new { controller = "Courses", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AdminDeals",
                url: "admin/deals/{action}/{id}",
                defaults: new { controller = "Deals", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
