using Ninject.Modules;
using MyBlog.DAL.Interfaces;
using MyBlog.DAL.Repository;

namespace MyBlog.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        string connectionString;
        public ServiceModule(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument(connectionString);
            Bind<IIdentityManager>().To<UnitOfWork>().WithConstructorArgument(connectionString);

        }
    }
}
