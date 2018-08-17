using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using MyBlog.BLL.Interfaces;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using MyBlog.WEB.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using AutoMapper;

namespace MyBlog.WEB.Controllers
{
    [Authorize(Roles ="Admin")]
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
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

            if(users != null)
            {
                return Ok(Mapper.Map<IEnumerable<UserViewModel>>(users));
            }

            return NotFound();
        }
        [Route("users/{id}")]
        public IHttpActionResult GetUser(string id)
        {
            var user = userService.GetUserById(id);

            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }

        [Route("posts")]
        public IHttpActionResult GetPosts([FromUri] int page = 1)
        {
            var posts = postService.GetPosts(page, false);
            if (posts != null)
            {
                return Ok(posts);
            }
            return NotFound();
        }
        [Route("posts/{id}")]
        public IHttpActionResult GetPost(int id)
        {
            var post = postService.GetPost(id);

            if (post != null)
            {
                return Ok(post);
            }
            return NotFound();
        }

        [Route("comments")]
        public IHttpActionResult GetComments([FromUri] int page = 1)
        {
            var comments = commentService.GetComments(page, 10, false);

            if (comments != null)
            {
                return Ok(comments);
            }
            return NotFound();
        }
        [Route("comments/{id}")]
        public IHttpActionResult GetComment(int id)
        {
            var comment = commentService.GetComment(id);

            if (comment != null)
            {
                return Ok(comment);
            }
            return NotFound();
        }
    }
}
