using System;

namespace MyBlog.DAL.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public int PostId { get; set; }
        public virtual Post Post { get; set; }

        public bool IsApproved { get; set; }


        public string AuthorId { get; set; }
        public virtual User Author { get; set; }

        public int ParentId { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
    }
}
