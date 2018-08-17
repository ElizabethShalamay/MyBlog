using MyBlog.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.Interfaces
{
    public interface ITagService
    {
        IEnumerable<TagDTO> GetAll();
        bool AddTag(TagDTO tagDTO);
        bool UpdateTag(TagDTO tagDTO);
    }
}
