using MyBlog.DAL.Entities;
using MyBlog.DAL.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //BlogUserManager UserManager { get; }
        //BlogRoleManager RoleManager { get; }
        //IRepository<Blog> BlogManager { get; }
        IRepository<Post> PostManager { get; }
        IRepository<Comment> CommentManager { get; }
        IRepository<Tag> TagManager { get; }

        Task SaveAsync();
    }
}
