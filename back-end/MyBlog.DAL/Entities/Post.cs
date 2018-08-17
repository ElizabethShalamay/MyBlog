using System;
using System.Collections.Generic;

namespace MyBlog.DAL.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }       
        public string Description { get; set; }
        public string Text { get; set; }

        public bool IsApproved { get; set; }

        public string UserId { get; set; }
        public virtual User Author { get; set; }

        public DateTime PostedAt { get; set; }       
        public virtual IList<Tag> Tags { get; set; }
    }
}
