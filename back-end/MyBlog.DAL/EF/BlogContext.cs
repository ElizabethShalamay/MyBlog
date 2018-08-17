using MyBlog.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.DAL
{
    public class BlogContext : IdentityDbContext<User>
    {

        public BlogContext()
            : base("BlogConnection", throwIfV1Schema: false)
        {
        }

        public static BlogContext Create()
        {
            return new BlogContext();
        }


        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
