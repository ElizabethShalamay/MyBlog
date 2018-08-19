using System.ComponentModel.DataAnnotations;

namespace MyBlog.WEB.Models
{
    public class TagViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tag should have name", AllowEmptyStrings = false)]       
        public string Name { get; set; }
    }
}