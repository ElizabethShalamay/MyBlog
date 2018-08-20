using AutoMapper;
using MyBlog.BLL.DTO;
using MyBlog.BLL.Interfaces;
using MyBlog.WEB.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace MyBlog.WEB.Controllers
{
    [Authorize]
    [RoutePrefix("api/comments")]
    public class CommentsController : ApiController
    {
        ICommentService commentService;

        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [Route("{postId}")]
        public IHttpActionResult GetComments(int postId, [FromUri] int page = 1)
        {
            IEnumerable<CommentDTO> comments = commentService.GetComments(postId, page, 10);
            IEnumerable<CommentViewModel> commentModels = Mapper.Map<IEnumerable<CommentViewModel>>(comments);
            
            return Ok(commentModels);
        }

        [Route("{postId}")]
        public IHttpActionResult PostComment(int postId, [FromBody] CommentViewModel comment)
        {
            if (comment == null)
            {
                return BadRequest();
            }

            comment.PostId = postId;
            CommentDTO commentDTO = Mapper.Map<CommentDTO>(comment);
            bool success = commentService.AddComment(commentDTO, User.Identity.Name);

            if (success)
                return Ok();
            return BadRequest();
        }

        [Route("{id}")]
        public IHttpActionResult PutComment(int id, [FromBody] CommentViewModel comment)
        {
            if (comment == null || id != comment.Id)
            {
                return BadRequest();
            }

            CommentDTO commentDTO = Mapper.Map<CommentDTO>(comment);
            bool success = commentService.UpdateComment(commentDTO);

            if (success)
                return Ok();
            return BadRequest();
        }

        public IHttpActionResult DeleteComment(int id)
        {
            bool success = commentService.DeleteComment(id);
            if (success)
                return Ok();
            return BadRequest();
        }
    }
}
