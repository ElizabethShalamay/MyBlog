using Autofac;
using MyBlog.DAL.Entities;
using MyBlog.DAL.Interfaces;
using MyBlog.DAL.Repository;

namespace DAL.Infrastructure
{
    public class DalModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Repository<Comment>>().As<IRepository<Comment>>().InstancePerRequest();
            builder.RegisterType<Repository<Post>>().As<IRepository<Post>>().InstancePerRequest();
            builder.RegisterType<Repository<Tag>>().As<IRepository<Tag>>().InstancePerRequest();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<UnitOfWork>().As<IIdentityManager>().InstancePerRequest();
        }
    }
}
