using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorId { get; set; }
        public int ParentId { get; set; }
        public DateTime? Date { get; set; }
        public string Text { get; set; }
        public bool IsApproved { get; set; }

        public IEnumerable<CommentDTO>  Children { get; set; }
    }
}
