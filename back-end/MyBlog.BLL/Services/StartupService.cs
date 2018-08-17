using MyBlog.DAL;
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
