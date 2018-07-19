using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    class Blog
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [ForeignKey("User")]
        public int AuthorId { get; set; }
        public virtual User Author { get; set; }

        public virtual IEnumerable<Post> Posts { get; set; }
    }
}
