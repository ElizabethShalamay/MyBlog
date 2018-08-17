using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.DTO
{
    public class PostDTO
    { 
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public string AuthorName { get; set; }
        public int Comments { get; set; }
        public DateTime? PostedAt { get; set; }
        public bool IsApproved { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}
