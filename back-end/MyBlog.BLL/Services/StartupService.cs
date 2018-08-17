using Microsoft.AspNet.Identity.EntityFramework;
using MyBlog.DAL;
using MyBlog.DAL.Entities;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using System;
using Owin;

namespace MyBlog.BLL.Services
{
    public static class StartupService
    {

        public static void ConfigureApplicationDbContext(this IAppBuilder app)
        {
            app.CreatePerOwinContext<BlogContext>(BlogContext.Create);
        }

        public static void ConfigureApplicationUserManager(this IAppBuilder app)
        {
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
        }
    }
}
