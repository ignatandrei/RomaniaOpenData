﻿using System;
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

        protected void Application_End()
        {
            
        }

        private void InitData()
        {
            var x = AppDomain.CurrentDomain.GetData("DataDirectory");

            //instanceRavenStore.BackupAllDatabase(@"D:\github\test");
            var judete = JudeteLoader.Judete().Result;
            var nume = judete.Select(it => it.Nume).OrderBy(it=>it).ToArray();
            Application["judete"] = nume;
            var data = DataSavedLoader.DataSaved().Result;
            Application["dataSaved"] = data;

        }
        
    }
}
