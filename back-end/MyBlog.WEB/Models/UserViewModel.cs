using System.ComponentModel.DataAnnotations;

namespace MyBlog.WEB.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "User should have name")]
        public string UserName { get; set; }

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage ="Email has invalid format")]
        public string Email { get; set; }
    }
}