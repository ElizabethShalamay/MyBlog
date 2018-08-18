using MyBlog.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using MyBlog.BLL.DTO;
using MyBlog.DAL.Interfaces;
using AutoMapper;
using MyBlog.DAL.Entities;

namespace MyBlog.BLL.Services
{
    /// <summary>
    /// Service for work with comments : reading, adding, editing, removing
    /// </summary>
    public class CommentService : ICommentService
    {
        IUnitOfWork Db { get; set; }
        public CommentService(IUnitOfWork unitOfWork)
        {
            Db = unitOfWork;
        }

        /// <summary>
        /// Find an existing comment by id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Comment with specific id</returns>
        CommentDTO ICommentService.GetComment(int id)
        {
            Comment comment = Db.CommentManager.Get(id);
            return Mapper.Map<Comment, CommentDTO>(comment);
        }

        /// <summary>
        /// Return list of comments for current page
        /// </summary>
        /// <param name="pageNum">Page number</param>
        /// <param name="pageSize">Amount of comments per page</param>
        /// <param name="approved">If comment should be approved to be chosen</param>
        /// <returns>Comments for current page</returns>
        IEnumerable<CommentDTO> ICommentService.GetComments(int pageNum, int pageSize, bool approved)
        {
            IEnumerable<Comment> comments = Db.CommentManager.Get().Where(c => c.IsApproved == approved)
                .OrderByDescending(p => p.Date)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize);

            IEnumerable<CommentDTO> commentsDTO = Mapper.Map<IEnumerable<CommentDTO>>(comments);

            //commentsDTO = CreateCommentTree(commentsDTO, approved);

            return commentsDTO;
        }

        /// <summary>
        /// Return list of comments for specific post
        /// </summary>
        /// <param name="postId">Post id</param>
        /// <param name="pageNum">Page number</param>
        /// <param name="pageSize">Amount of comments per page</param>
        /// <param name="approved">If comments should be approved to be chosen</param>
        /// <returns>List of comments for specific post for current page</returns>
        IEnumerable<CommentDTO> ICommentService.GetComments(int postId, int pageNum, int pageSize, bool approved)
        {
            IEnumerable<Comment> comments = Db.CommentManager.Get().Where(c => c.PostId == postId && c.IsApproved == approved)
                .OrderByDescending(p => p.Date)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize);

            IEnumerable<CommentDTO> commentsDTO = Mapper.Map<IEnumerable<CommentDTO>>(comments);

            commentsDTO = CreateCommentTree(commentsDTO, approved);

            return commentsDTO;
        }

        /// <summary>
        /// Add a new comment
        /// </summary>
        /// <param name="commentDTO">Comment</param>
        /// <param name="userName">Author name</param>
        /// <returns>Boolean value of operatin success</returns>
        bool ICommentService.AddComment(CommentDTO commentDTO, string userName)
        {
            if (commentDTO == null)
                return false;

            Comment comment = Mapper.Map<CommentDTO, Comment>(commentDTO);
            comment.Date = DateTime.Now;
            comment.AuthorId = (Db as IIdentityManager).AppUserManager.FindByNameAsync(userName).Result.Id;

            int result = Db.CommentManager.Create(comment);
            return result > 0;
        }

        /// <summary>
        /// Remove comment with specified id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Boolean value of operatin success</returns>
        bool ICommentService.DeleteComment(int id)
        {
            int result = Db.CommentManager.Delete(id);
            return result > 0;
        }

        /// <summary>
        /// Edit an existing comment
        /// </summary>
        /// <param name="commentDTO">Comment</param>
        /// <returns>Boolean value of operatin success</returns>
        bool ICommentService.UpdateComment(CommentDTO commentDTO)
        {
            if (commentDTO == null)
                return false;

            Comment comment = Mapper.Map<Comment>(commentDTO);

            int result = Db.CommentManager.Update(comment);
            return result > 0;
        }

        private IEnumerable<CommentDTO> CreateCommentTree(IEnumerable<CommentDTO> commentsDTO, bool approved)
        {
            List<CommentDTO> comments = approved? commentsDTO.Where(c => c.ParentId == 0).ToList() : commentsDTO.ToList() ;
            comments.ForEach(comment=> comment.Children = commentsDTO.Where(c => c.ParentId == comment.Id));
            return comments;
        }
    }
}
