using DAL.Entities;
using DAL.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    interface IUnitOfWork : IDisposable
    {
        BlogUserManager UserManager { get; }
        BlogRoleManager RoleManager { get; }
        IRepository<Post> PostManager { get; }
        IRepository<Comment> CommentManager { get; }
        IRepository<Tag> TagManager { get; }


        Task SaveAsync();
    }
}
