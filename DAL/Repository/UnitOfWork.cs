using DAL.EF;
using DAL.Entities;
using DAL.Identity;
using DAL.Repository;
using DAL.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    // TODO: if necessary, another unit of work (for identity)
    class UnitOfWork : IUnitOfWork, IDisposable
    {
        BlogContext blog = new BlogContext();
        BlogUserManager userManager;
        BlogRoleManager roleManager;
        IRepository<Post> postManager;
        IRepository<Comment> commentManager;
        IRepository<Tag> tagManager;

        public UnitOfWork()
        {
            blog = new BlogContext();
            userManager = new BlogUserManager(new UserStore<User>(blog));
            roleManager = new BlogRoleManager(new RoleStore<Role>(blog));
            postManager = new Repository<Post>(blog);
            commentManager = new Repository<Comment>(blog);
            tagManager = new Repository<Tag>(blog);
        }
        
        BlogUserManager IUnitOfWork.UserManager => userManager;

        BlogRoleManager IUnitOfWork.RoleManager => roleManager;

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
