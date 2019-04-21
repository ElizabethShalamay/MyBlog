using Autofac;
using Autofac.Integration.WebApi;
using MyBlog.BLL.Infrastructure;
using System.Reflection;
using Module = Autofac.Module;

namespace MyBlog.WEB.Infrastructure
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterModule<ServiceModule>();
        }
    }
}