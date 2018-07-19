using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    class Comment
    {
        public string Id { get; set; }

        public string PostId { get; set; }
        public virtual Post Post { get; set; }

        public string AuthorId { get; set; }
        public virtual User Author { get; set; }

        public string ParentId { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
    }
}
