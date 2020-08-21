using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Elmah;
using TicketManagement.MapperConfig;
using GlobalConfiguration = System.Web.Http.GlobalConfiguration;
using TicketManagement.Concrete.CacheLibrary;

namespace TicketManagement
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfiguration.Config();
            MenuCacheHelper menuCacheHelper = new MenuCacheHelper();
            menuCacheHelper.LoadMenu();
            BundleTable.EnableOptimizations = false;

        }

        void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            //log the error!
            ErrorSignal.FromCurrentContext().Raise(ex);
            
            Response.Redirect($"~/Error/Error");
        }
    }
}
