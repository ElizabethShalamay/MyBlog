using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using MyBlog.BLL.DTO;
using MyBlog.BLL.Interfaces;
using MyBlog.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace MyBlog.WEB.Controllers
{
    [Authorize]
    public class CommentsController : ApiController
    {
        ICommentService commentService;

        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [Route("api/comments/{postId}")]
        public IEnumerable<CommentViewModel> GetComments([FromUri] int page = 1)
        {
            var uri = Request.RequestUri;
            int postId = Convert.ToInt32(uri.Segments[3]);
            var comments = commentService.GetComments(postId, page, 10);
            comments = SetAuthorsNames(comments);
            return Mapper.Map<IEnumerable<CommentDTO>, IEnumerable<CommentViewModel>>(comments);
        }       

        [Route("api/comments/{postId}")]
        public void PostComment([FromBody] CommentViewModel comment)
        {
            var uri = Request.RequestUri;
            int postId = Convert.ToInt32(uri.Segments[3]);
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindByNameAsync(User.Identity.Name);
            comment.AuthorId = user.Result.Id;
            comment.PostId = postId;
            commentService.AddComment(Mapper.Map<CommentViewModel, CommentDTO>(comment));
        }

        [Route("api/comments/{id}")]
        public void PutComment(int id, [FromBody] CommentViewModel comment)
        {
            if (id == comment.Id)
            {
                var commentDTO = Mapper.Map<CommentViewModel, CommentDTO>(comment);
                commentService.UpdateComment(commentDTO);
            }
        }

        [Route("api/comments")]
        public void DeleteComment(int id)
        {
            commentService.DeleteComment(id);
        }


        private IEnumerable<CommentDTO> SetAuthorsNames(IEnumerable<CommentDTO> comments)
        {
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            foreach(var comment in comments)
            {
                comment.AuthorName = userManager.FindByIdAsync(comment.AuthorId).Result.UserName;
                SetAuthorsNames(comment.Children);
            }
            return comments;
        }
    }
}
