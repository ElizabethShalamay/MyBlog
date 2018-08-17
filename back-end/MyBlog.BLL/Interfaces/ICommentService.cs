using MyBlog.BLL.DTO;
using System.Collections.Generic;

namespace MyBlog.BLL.Interfaces
{
    public interface ICommentService
    {
        CommentDTO GetComment(int id);
        IEnumerable<CommentDTO> GetComments(int postId, int pageNum, int pageSize, bool approved = true);
        IEnumerable<CommentDTO> GetComments(int pageNum, int pageSize, bool approved = true);

        bool AddComment(CommentDTO postDTO, string userName);
        bool UpdateComment(CommentDTO comment);
        bool DeleteComment(int id);
    }
}
