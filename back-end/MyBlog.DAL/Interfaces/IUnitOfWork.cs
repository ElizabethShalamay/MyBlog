using Microsoft.AspNet.Identity;
using MyBlog.DAL.Entities;
using System;

namespace MyBlog.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        BlogContext Blog { get; }
        IRepository<Post> PostManager { get; }
        IRepository<Comment> CommentManager { get; }
        IRepository<Tag> TagManager { get; }

        UserManager<User> AppUserManager { get; }
        RoleManager<Role> AppRoleManager { get; }      
    }
}
