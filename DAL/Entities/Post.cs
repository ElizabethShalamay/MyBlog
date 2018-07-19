using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }       
        public string Description { get; set; }

        //public virtual string UrlSlug { get; set; }  // for addressing

        public int UserId { get; set; }
        public virtual User Author { get; set; }

        public virtual DateTime PostedOn { get; set; }       
        public virtual IList<Tag> Tags { get; set; }
    }
}
