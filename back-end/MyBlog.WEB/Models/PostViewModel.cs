using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.WEB.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Post should have title", AllowEmptyStrings = false)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Post should have description", AllowEmptyStrings = false)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Post should have author", AllowEmptyStrings = false)]
        public string UserId { get; set; }
        public string AuthorName { get; set; }
        public DateTime? PostedAt { get; set; }
        [Required(ErrorMessage = "Post should have text", AllowEmptyStrings = false)]
        public string Text { get; set; }
        public bool IsApproved { get; set; }
        public int Comments { get; set; }
        public List<string> Tags { get; set; }
    }
}