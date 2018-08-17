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
            Comment comment = Db.CommentManager.Get(id);
            return Mapper.Map<Comment, CommentDTO>(comment);
        }

        bool ICommentService.AddComment(CommentDTO commentDTO, string userName)
        {
            if (commentDTO == null)
                return false;

            Comment comment = Mapper.Map<CommentDTO, Comment>(commentDTO);
            comment.Date = DateTime.Now;
            comment.AuthorId = Db.AppUserManager.FindByNameAsync(userName).Result.Id;

            int result = Db.CommentManager.Create(comment);
            return result > 0;
        }

        bool ICommentService.DeleteComment(int id)
        {
            int result = Db.CommentManager.Delete(id);
            return result > 0;
        }

        IEnumerable<CommentDTO> ICommentService.GetComments(int pageNum, int pageSize, bool approved)
        {
            IEnumerable<Comment> comments = Db.CommentManager.Get().Where(c => c.IsApproved == approved)
                .OrderByDescending(p => p.Date)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize);

            IEnumerable<CommentDTO> commentsDTO = Mapper.Map<IEnumerable<CommentDTO>>(comments);
            foreach (CommentDTO comment in commentsDTO)
            {
                comment.Children = commentsDTO.Where(c => c.ParentId == comment.Id).AsEnumerable();
            }
            commentsDTO = commentsDTO.Where(c => c.ParentId == 0);
            return commentsDTO;
        }

        IEnumerable<CommentDTO> ICommentService.GetComments(int postId, int pageNum, int pageSize, bool approved)
        {
            IEnumerable<Comment> comments = Db.CommentManager.Get().Where(c => c.PostId == postId && c.IsApproved == approved)
                .OrderByDescending(p => p.Date)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize);
            IEnumerable<CommentDTO> commentsDTO = Mapper.Map<IEnumerable<CommentDTO>>(comments);
            foreach (CommentDTO comment in commentsDTO)
            {
                comment.Children = commentsDTO.Where(c => c.ParentId == comment.Id).AsEnumerable();
            }
            commentsDTO = commentsDTO.Where(c => c.ParentId == 0);
            return commentsDTO;
        }

        bool ICommentService.UpdateComment(CommentDTO commentDTO)
        {
            if (commentDTO != null)
                return false;

            Comment comment = Mapper.Map<Comment>(commentDTO);

            int result = Db.CommentManager.Update(comment, comment.Id);
            return result > 0;
        }

        private IEnumerable<CommentDTO> SetAuthorsNames(IEnumerable<CommentDTO> comments) // use automapper!
        {
            foreach (CommentDTO comment in comments)
            {
                comment.AuthorName = Db.AppUserManager.FindByIdAsync(comment.AuthorId).Result.UserName;
                SetAuthorsNames(comment.Children);
            }
            return comments;
        }

        /// build tree view commnets private method used in GetComments
    }
}
