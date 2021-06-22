using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace onlineCourses
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            /*config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "GetAccount",
                routeTemplate: "api/{controller}/{username}/{pwd}",
                defaults: new { username = RouteParameter.Optional, pwd = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "PutAccount",
                routeTemplate: "api/{controller}/{id}/{account}",
                defaults: new { id = RouteParameter.Optional, account = RouteParameter.Optional }
            );*/
        }
    }
}
