using Autofac;
using MyBlog.BLL.Services;
using MyBlog.BLL.Interfaces;
using Module = Autofac.Module;
using DAL.Infrastructure;

namespace MyBlog.BLL.Infrastructure
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommentService>().As<ICommentService>().InstancePerRequest();
            builder.RegisterType<PostService>().As<IPostService>().InstancePerRequest();
            builder.RegisterType<TagService>().As<ITagService>().InstancePerRequest();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();

            builder.RegisterModule<DalModule>();
        }
    }
}
