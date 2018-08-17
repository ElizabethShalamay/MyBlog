using MyBlog.BLL.DTO;
using System.Collections.Generic;

namespace MyBlog.BLL.Interfaces
{
    public interface IPostService
    {
        PageInfo PageInfo { get; set; }

        PostDTO GetPost(int id);
        IEnumerable<PostDTO> GetPosts(int page, bool approved);
        IEnumerable<PostDTO> GetPosts(string authorName, int pageNum);

        IEnumerable<PostDTO> GetPostsByAuthor(string id);
        IEnumerable<PostDTO> GetPostsByText(string text);
        IEnumerable<PostDTO> GetPostsByTag(string tag);

        bool AddPost(PostDTO postDTO, string userName, IList<string> tags);
        bool DeletePost(int id);
        bool UpdatePost(PostDTO postDTO);

        int CountComments(int postId);
        string GetPaginationData(int page);
    }
}
