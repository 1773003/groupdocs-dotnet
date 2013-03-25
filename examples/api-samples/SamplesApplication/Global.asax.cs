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
                     "sample11", // Route name
                     "{controller}/{action}/{result}", // URL with parameters
                     new { controller = "Samples", action = "sample11", result = UrlParameter.Optional } // Parameter defaults
                   );

              routes.MapRoute(
                       "sample12", // Route name
                       "{controller}/{action}/{result}", // URL with parameters
                       new { controller = "Samples", action = "sample12", result = UrlParameter.Optional } // Parameter defaults
                     );

              routes.MapRoute(
                         "sample13", // Route name
                         "{controller}/{action}/{result}", // URL with parameters
                         new { controller = "Samples", action = "sample13", result = UrlParameter.Optional } // Parameter defaults
                       );

              routes.MapRoute(
                          "sample14", // Route name
                          "{controller}/{action}/{result}", // URL with parameters
                          new { controller = "Samples", action = "sample14", result = UrlParameter.Optional } // Parameter defaults
                        );
              routes.MapRoute(
                         "sample15", // Route name
                         "{controller}/{action}/{result}", // URL with parameters
                         new { controller = "Samples", action = "sample15", result = UrlParameter.Optional } // Parameter defaults
                       );

              routes.MapRoute(
                           "sample16", // Route name
                           "{controller}/{action}/{result}", // URL with parameters
                           new { controller = "Samples", action = "sample16", result = UrlParameter.Optional } // Parameter defaults
                         );

              routes.MapRoute(
                             "sample17", // Route name
                             "{controller}/{action}/{result}", // URL with parameters
                             new { controller = "Samples", action = "sample17", result = UrlParameter.Optional } // Parameter defaults
                           );

              routes.MapRoute(
                    "sample18", // Route name
                    "{controller}/{action}/{result}", // URL with parameters
                    new { controller = "Samples", action = "sample18", result = UrlParameter.Optional } // Parameter defaults
                  );

              routes.MapRoute(
                     "sample19", // Route name
                     "{controller}/{action}/{result}", // URL with parameters
                     new { controller = "Samples", action = "sample19", result = UrlParameter.Optional } // Parameter defaults
                   );

              routes.MapRoute(
                        "sample20", // Route name
                        "{controller}/{action}/{result}", // URL with parameters
                        new { controller = "Samples", action = "sample20", result = UrlParameter.Optional } // Parameter defaults
                      );

              routes.MapRoute(
                         "sample21", // Route name
                         "{controller}/{action}/{result}", // URL with parameters
                         new { controller = "Samples", action = "sample21", result = UrlParameter.Optional } // Parameter defaults
                       );

              routes.MapRoute(
                          "sample22", // Route name
                          "{controller}/{action}/{result}", // URL with parameters
                          new { controller = "Samples", action = "sample22", result = UrlParameter.Optional } // Parameter defaults
                        );

              routes.MapRoute(
                            "sample23", // Route name
                            "{controller}/{action}/{result}", // URL with parameters
                            new { controller = "Samples", action = "sample23", result = UrlParameter.Optional } // Parameter defaults
                          );

              routes.MapRoute(
                            "sample24", // Route name
                            "{controller}/{action}/{result}", // URL with parameters
                            new { controller = "Samples", action = "sample24", result = UrlParameter.Optional } // Parameter defaults
                          );

              routes.MapRoute(
                       "compare_callback", // Route name
                       "{controller}/{action}/{result}", // URL with parameters
                       new { controller = "Samples", action = "compare_callback", result = UrlParameter.Optional } // Parameter defaults
                     );

              routes.MapRoute(
                        "signature_callback", // Route name
                        "{controller}/{action}/{result}", // URL with parameters
                        new { controller = "Samples", action = "signature_callback", result = UrlParameter.Optional } // Parameter defaults
                      );

              routes.MapRoute(
                          "annotation_callback", // Route name
                          "{controller}/{action}/{result}", // URL with parameters
                          new { controller = "Samples", action = "annotation_callback", result = UrlParameter.Optional } // Parameter defaults
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