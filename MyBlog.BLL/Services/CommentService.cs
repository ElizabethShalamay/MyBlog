using MyBlog.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBlog.BLL.DTO;
using MyBlog.DAL.Interfaces;
using AutoMapper;
using MyBlog.DAL.Entities;

namespace MyBlog.BLL.Services
{
    public class CommentService : ICommentService
    {
        IUnitOfWork Db { get; set; }
        public CommentService(IUnitOfWork unitOfWork)
        {
            Db = unitOfWork;
        }

        CommentDTO ICommentService.GetComment(int id)
        {
            var comment = Db.CommentManager.Get(id);
            return Mapper.Map<Comment, CommentDTO>(comment);
        }

        void ICommentService.AddComment(CommentDTO commentDTO)
        {
            if (commentDTO != null)
            {
                var comment = Mapper.Map<CommentDTO, Comment>(commentDTO);
                comment.Date = DateTime.Now;
                Db.CommentManager.Create(comment);
            }
        }      

        void ICommentService.DeleteComment(int id)
        {
            Db.CommentManager.Delete(id);
            Db.SaveAsync();
        }

        IEnumerable<CommentDTO> ICommentService.GetComments(int pageNum, int pageSize, bool approved)
        {
            var comments = Db.CommentManager.Get().Where(c => c.IsApproved == approved)
                .OrderByDescending(p => p.Date)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize);
            var commentsDTO = Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(comments);
            foreach (var comment in commentsDTO)
            {
                comment.Children = commentsDTO.Where(c => c.ParentId == comment.Id).AsEnumerable();
            }
            commentsDTO = commentsDTO.Where(c => c.ParentId == 0);
            return commentsDTO;
        }

        IEnumerable<CommentDTO> ICommentService.GetComments(int postId, int pageNum, int pageSize, bool approved)
        {
            var comments = Db.CommentManager.Get().Where(c => c.PostId == postId && c.IsApproved == approved)
                .OrderByDescending(p => p.Date)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize);
            var commentsDTO = Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(comments);
            foreach(var comment in commentsDTO)
            {
                comment.Children = commentsDTO.Where(c => c.ParentId == comment.Id).AsEnumerable();
            }
            commentsDTO = commentsDTO.Where(c => c.ParentId == 0);
            return commentsDTO;
        }

        void ICommentService.UpdateComment(CommentDTO commentDTO)
        {
            if (commentDTO != null)
            {
                var comment = Mapper.Map<CommentDTO, Comment>(commentDTO);
                Db.CommentManager.Update(comment, comment.Id);
            }
        }
    }
}
