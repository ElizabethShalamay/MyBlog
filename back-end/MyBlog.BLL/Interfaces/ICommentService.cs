using MyBlog.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.Interfaces
{
    public interface ICommentService
    {
        CommentDTO GetComment(int id);
        IEnumerable<CommentDTO> GetComments(int postId, int pageNum, int pageSize, bool approved = true);
        IEnumerable<CommentDTO> GetComments(int pageNum, int pageSize, bool approved = true);

        void AddComment(CommentDTO postDTO, string userName);
        void UpdateComment(CommentDTO comment);
        void DeleteComment(int id);
    }
}
