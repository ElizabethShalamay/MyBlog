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
    }
}
