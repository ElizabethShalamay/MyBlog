using AutoMapper;
using MyBlog.BLL.Infrastructure;
using MyBlog.WEB.App_Start;
using MyBlog.WEB.Util;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MyBlog.WEB
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var config = App_Start.MappingConfig.InitializeMapper();
            BLL.Infrastructure.MappingConfig.InitializeMapper(config);
        }
    }
}
