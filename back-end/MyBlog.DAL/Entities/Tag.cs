using DAL.Entities;
using System.Collections.Generic;

namespace MyBlog.DAL.Entities
{
    public class Tag : IIdentical
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual IList<Post> Posts { get; set; } 
    }
}
