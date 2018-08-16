using MyBlog.DAL.EF;
using MyBlog.DAL.Entities;
using MyBlog.DAL.Repository;
using MyBlog.DAL.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        BlogContext blog = new BlogContext();
        IRepository<Post> postManager;
        IRepository<Comment> commentManager;
        IRepository<Tag> tagManager;

        public UnitOfWork()
        {
            blog = new BlogContext();
            postManager = new Repository<Post>(blog);
            commentManager = new Repository<Comment>(blog);
            tagManager = new Repository<Tag>(blog);
        }

        IRepository<Post> IUnitOfWork.PostManager => postManager;
        IRepository<Comment> IUnitOfWork.CommentManager => commentManager;
        IRepository<Tag> IUnitOfWork.TagManager => tagManager;

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    blog.Dispose();
                }
                disposed = true;
            }
        }
        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        async Task IUnitOfWork.SaveAsync()
        {
            await blog.SaveChangesAsync();
        }
    }
}
