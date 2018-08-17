using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MyBlog.WEB.Startup))]

namespace MyBlog.WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
