using MyBlog.BLL.DTO;
using System.Collections.Generic;

namespace MyBlog.BLL.Interfaces
{
    public interface ITagService
    {
        IEnumerable<TagDTO> GetTop();
        IEnumerable<TagDTO> GetAll();

        bool AddTag(TagDTO tagDTO);
        bool UpdateTag(TagDTO tagDTO);
    }
}
