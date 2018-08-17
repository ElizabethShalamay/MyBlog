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
    // add route prefix
    // use ihttpactionresult
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
            int postId = Convert.ToInt32(uri.Segments[3]); // ??
            var comments = commentService.GetComments(postId, page, 10);
            return Mapper.Map<IEnumerable<CommentDTO>, IEnumerable<CommentViewModel>>(comments);
        }       

        [Route("api/comments/{postId}")]
        public void PostComment(int postId, [FromBody] CommentViewModel comment)
        {
            comment.PostId = postId;
            commentService.AddComment(Mapper.Map<CommentViewModel, CommentDTO>(comment), User.Identity.Name);
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
        public void DeleteComment(int id) // async + ihhtpactionresult for all with valiation
        {
            commentService.DeleteComment(id);
        }      
    }
}
