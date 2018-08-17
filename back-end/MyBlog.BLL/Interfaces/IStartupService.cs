using MyBlog.DAL;

namespace MyBlog.BLL.Interfaces
{
    public interface IStartupService
    {
        BlogContext CreateContext();
        ApplicationUserManager CreateUserManager();
    }
}
