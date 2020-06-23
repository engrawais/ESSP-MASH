using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using Autofac;
using AppESSP.Modules;
using Newtonsoft.Json;
using ESSPCORE.Reporting;
using System.Web;
using System;
using ESSPCORE.Common;
using ESSPCORE.Attendance;
using System.Collections.Generic;

namespace AppESSP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Autofac Configuration
            var builder = new Autofac.ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new EFModule());
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            // Json default settings which only get parent entries not child 
            //JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            //{
            //    Formatting = Newtonsoft.Json.Formatting.Indented,
            //    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //};
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            HttpContext.Current.Session["FiltersModel"] = new VMSelectedFilter();
            Session["VMATDashboard"] = "";
            Session["VMFeedbackDashboardSession"] = "";
            HttpContext.Current.Session["ToasterMessages"] = new List<string>();
            //LoadSession();
        }
        protected void Session_End(object sender, EventArgs e)
        {
            HttpContext.Current.Session["FiltersModel"] = null;
            HttpContext.Current.Session["LoggedInUser"] = null;
            HttpContext.Current.Session["VMATDashboard"] = null;
            HttpContext.Current.Session["VMFeedbackDashboardSession"] = null;
            HttpContext.Current.Session["FiltersModelRMS"] = null;
            HttpContext.Current.Session["ToasterMessages"] = null;
        }
    }
}
