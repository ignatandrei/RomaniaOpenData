using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ROPInfrastructure;

namespace ROPWebMVC
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
            InitData();
        }

        private void InitData()
        {
            //instanceRavenStore.BackupAllDatabase(@"D:\github\test");
            var judete = instanceRavenStore.Judete().Result;
            var nume = judete.Select(it => it.Nume).OrderBy(it=>it).ToArray();
            Application["judete"] = nume;
            var data = instanceRavenStore.DataSaved().Result;
            Application["dataSaved"] = data;

        }
        
    }
}
