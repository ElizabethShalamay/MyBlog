using MyBlog.DAL.Entities;
using MyBlog.DAL.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MyBlog.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        BlogContext blog;
        IRepository<Post> postManager;
        IRepository<Comment> commentManager;
        IRepository<Tag> tagManager;

        ApplicationUserManager userManager;
        RoleManager<Role> roleManager;
       

        public UnitOfWork()
        {
            blog = new BlogContext();

            postManager = new Repository<Post>(blog);
            commentManager = new Repository<Comment>(blog);
            tagManager = new Repository<Tag>(blog);

            userManager = new ApplicationUserManager(new UserStore<User>(blog));
            roleManager = new RoleManager<Role>(new RoleStore<Role>());
        }

        IRepository<Post> IUnitOfWork.PostManager => postManager;
        IRepository<Comment> IUnitOfWork.CommentManager => commentManager;
        IRepository<Tag> IUnitOfWork.TagManager => tagManager;

        UserManager<User> IUnitOfWork.AppUserManager => userManager;
        RoleManager<Role> IUnitOfWork.AppRoleManager => roleManager;

        BlogContext IUnitOfWork.Blog => blog;

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
