using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.WEB.Models
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string AuthorName { get; set; }
        [Required(ErrorMessage ="Comment should have author")]
        public string AuthorId { get; set; }
        public int ParentId { get; set; }
        public DateTime? Date { get; set; }
        [Required(ErrorMessage ="Comment should have text", AllowEmptyStrings = false)]
        public string Text { get; set; }
        public bool IsApproved { get; set; }

        public IEnumerable<CommentViewModel> Children { get; set; }
    }
}