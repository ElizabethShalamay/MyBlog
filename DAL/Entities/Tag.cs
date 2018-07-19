using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual IList<Post> Posts { get; set; } 
    }
}
