using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.WEB.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string AuthorName { get; set; }
        public DateTime? PostedAt { get; set; }
        public string Text { get; set; }
        public bool IsApproved { get; set; }
        public int Comments { get; set; }
        public List<string> Tags { get; set; }
    }
}