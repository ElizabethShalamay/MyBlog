using MyBlog.BLL.Interfaces;
using MyBlog.BLL.Services;
using Ninject.Modules;

namespace MyBlog.WEB.Util
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IPostService>().To<PostService>();
            Bind<ICommentService>().To<CommentService>();
            Bind<ITagService>().To<TagService>();
            Bind<IUserService>().To<UserService>();
        }
    }
}