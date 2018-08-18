using Microsoft.AspNet.Identity;
using MyBlog.DAL.Entities;

namespace MyBlog.DAL.Interfaces
{
    public interface IIdentityManager
    {
        UserManager<User> AppUserManager { get; }
        RoleManager<Role> AppRoleManager { get; }
    }
}
