using MyBlog.DAL;
using Owin;

namespace MyBlog.BLL.Services
{
    /// <summary>
    /// Service for startup OWIN context configuration
    /// </summary>
    public static class StartupService
    {
        /// <summary>
        /// Define a way for instantiating an application context
        /// </summary>
        /// <param name="app"></param> 
        public static void ConfigureApplicationDbContext(this IAppBuilder app)
        {
            app.CreatePerOwinContext<BlogContext>(BlogContext.Create);
        }

        /// <summary>
        /// Define a way for instantiating an application user manager
        /// </summary>
        /// <param name="app"></param> 
        public static void ConfigureApplicationUserManager(this IAppBuilder app)
        {
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
        }
    }
}
