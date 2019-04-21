using DAL.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlog.DAL.Entities
{
    public class Tag : IIdentical
    {
        public int Id { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(450)]
        [Index]
        public string Name { get; set; }
        public virtual IList<Post> Posts { get; set; } 
    }
}
