using System.Collections.Generic;
using MyBlog.BLL.Interfaces;
using System.Web.Http;
using MyBlog.WEB.Models;
using AutoMapper;

namespace MyBlog.WEB.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
        const int ADMIN_PAGE_SIZE = 15;

        IPostService postService;
        ICommentService commentService;
        IUserService userService;

        public AdminController(IPostService postService, ICommentService commentService, IUserService userService)
        {
            this.postService = postService;
            this.commentService = commentService;
            this.userService = userService;
        }

        [Route("users")]
        public IHttpActionResult GetUsers([FromUri] int page)
        {
            var users = userService.GetUsers(page);
            return Ok(Mapper.Map<IEnumerable<UserViewModel>>(users));
        }

        [Route("users/{id}")]
        public IHttpActionResult GetUser(string id)
        {
            var user = userService.GetUserById(id);
            if (user != null)
                return Ok(Mapper.Map<UserViewModel>(user));
            return NotFound();
        }

        [Route("posts")]
        public IHttpActionResult GetPosts([FromUri] int page = 1)
        {
            postService.PageInfo.PageNumber = page;
            postService.PageInfo.PageSize = ADMIN_PAGE_SIZE;

            var posts = postService.GetPosts(page, false);
            return Ok(Mapper.Map<IEnumerable<PostViewModel>>(posts));
        }

        [Route("posts/{id}")]
        public IHttpActionResult GetPost(int id)
        {
            var post = postService.GetPost(id);
            if (post != null)
                return Ok(Mapper.Map<PostViewModel>(post));
            return NotFound();
        }

        [Route("comments")]
        public IHttpActionResult GetComments([FromUri] int page = 1)
        {
            var comments = commentService.GetComments(page, 10, false);
            return Ok(Mapper.Map<IEnumerable<CommentViewModel>>(comments));
        }

        [Route("comments/{id}")]
        public IHttpActionResult GetComment(int id)
        {
            var comment = commentService.GetComment(id);
            if (comment != null)
                return Ok(Mapper.Map<CommentViewModel>(comment));
            return NotFound();
        }
    }
}
