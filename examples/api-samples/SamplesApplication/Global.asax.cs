using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SamplesApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
                routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

                routes.MapRoute(
                    "Default", // Route name
                    "{controller}/{action}/{id}", // URL with parameters
                    new { controller = "Samples", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                );

                routes.MapRoute(
                    "sample01", // Route name
                    "{controller}/{action}/{result}", // URL with parameters
                    new { controller = "Samples", action = "sample01", result = UrlParameter.Optional } // Parameter defaults
                );

                routes.MapRoute(
                    "sample02", // Route name
                    "{controller}/{action}/{result}", // URL with parameters
                    new { controller = "Samples", action = "sample02", result = UrlParameter.Optional } // Parameter defaults
                );

                routes.MapRoute(
                   "sample03", // Route name
                   "{controller}/{action}/{result}", // URL with parameters
                   new { controller = "Samples", action = "sample03", result = UrlParameter.Optional } // Parameter defaults
               );

               routes.MapRoute(
                   "sample04", // Route name
                   "{controller}/{action}/{result}", // URL with parameters
                   new { controller = "Samples", action = "sample04", result = UrlParameter.Optional } // Parameter defaults
               );

               routes.MapRoute(
                  "sample05", // Route name
                  "{controller}/{action}/{result}", // URL with parameters
                  new { controller = "Samples", action = "sample05", result = UrlParameter.Optional } // Parameter defaults
              );

               routes.MapRoute(
                 "sample06", // Route name
                 "{controller}/{action}/{result}", // URL with parameters
                 new { controller = "Samples", action = "sample06", result = UrlParameter.Optional } // Parameter defaults
             );

               routes.MapRoute(
                "sample07", // Route name
                "{controller}/{action}/{result}", // URL with parameters
                new { controller = "Samples", action = "sample07", result = UrlParameter.Optional } // Parameter defaults
            );

              routes.MapRoute(
               "sample08", // Route name
               "{controller}/{action}/{result}", // URL with parameters
               new { controller = "Samples", action = "sample08", result = UrlParameter.Optional } // Parameter defaults
             );

              routes.MapRoute(
                "sample09", // Route name
                "{controller}/{action}/{result}", // URL with parameters
                new { controller = "Samples", action = "sample09", result = UrlParameter.Optional } // Parameter defaults
              );

              routes.MapRoute(
                  "sample10", // Route name
                  "{controller}/{action}/{result}", // URL with parameters
                  new { controller = "Samples", action = "sample10", result = UrlParameter.Optional } // Parameter defaults
                );

              routes.MapRoute(
                    "sample18", // Route name
                    "{controller}/{action}/{result}", // URL with parameters
                    new { controller = "Samples", action = "sample18", result = UrlParameter.Optional } // Parameter defaults
                  );
                        
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}