using MyBlog.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.Interfaces
{
    public interface IPostService
    {
        PageInfo PageInfo { get; set; }

        PostDTO GetPost(int id);
        IEnumerable<PostDTO> GetPosts(int page, bool approved);
        IEnumerable<PostDTO> GetPosts(string authorId, int pageNum);

        IEnumerable<PostDTO> GetPostsByAuthor(string id);
        IEnumerable<PostDTO> GetPostsByText(string text);
        IEnumerable<PostDTO> GetPostsByTag(string tag);

        void AddPost(PostDTO postDTO, IList<string> tags);
        void DeletePost(int id);

        int CountComments(int postId);
    }
}
