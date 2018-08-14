using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using MyBlog.BLL.DTO;
using MyBlog.BLL.Interfaces;
using MyBlog.WEB.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MyBlog.WEB.Controllers
{
    [Authorize(Roles ="User")]
    public class PostsController : ApiController
    {
        IPostService postService;
        ApplicationUserManager userManager;
        public PostsController(IPostService postService)
        {
            this.postService = postService;
        }
        

        [Route("api/posts")]
        public IHttpActionResult GetPosts([FromUri] int page = 1)
        {
            var user = GetUser(User.Identity.Name);
            var postsDTO = postService.GetPosts(user.Id, page);
            var posts = Mapper.Map<IEnumerable<PostDTO>, IEnumerable<PostViewModel>>(postsDTO);

            // Setting Header  
            HttpContext.Current.Response.Headers.Add("Paging-Headers",GetPaginationData(page));
            foreach (var post in posts)
            {
                post.AuthorName = SetAuthorName(post.UserId);
            }
            return Ok(posts);
        }

        [Route("api/posts")]
        public IEnumerable<PostViewModel> GetPosts([FromUri] string text)
        {
            IEnumerable<PostDTO> postsDTO = null;
            text = HttpUtility.UrlDecode(text);
            if (text.StartsWith("#"))
                postsDTO = postService.GetPostsByTag(text);
            else postsDTO = postService.GetPostsByText(text);

            var posts = Mapper.Map<IEnumerable<PostDTO>, IEnumerable<PostViewModel>>(postsDTO);
            foreach (var post in posts)
            {
                post.AuthorName = SetAuthorName(post.UserId);
            }
            return posts;
        }

        [Route("api/posts/{id}")]
        public PostViewModel GetPost(int id)
        {
            var postDTO = postService.GetPost(id);
            var post = Mapper.Map<PostDTO, PostViewModel>(postDTO);
            post.AuthorName = SetAuthorName(post.UserId);
            return post;
        }

        [Route("api/posts")]
        public void Post([FromBody] PostViewModel post)
        {
            post.UserId = GetUser(User.Identity.Name).Id;
            postService.AddPost(Mapper.Map<PostViewModel, PostDTO>(post), post.Tags);
        }

        [Route("api/posts/{id}")]
        public void Put(int id, [FromBody] PostViewModel post)
        {
        }

        [Route("api/posts/{id}")]
        public void Delete(int id)
        {
            postService.DeletePost(id);
        }

        [Route("api/posts/news")]
        public IEnumerable<PostViewModel> GetNews([FromUri] int page = 1)
        {
            var postsDTO = postService.GetPosts(page, true);
            var posts = Mapper.Map<IEnumerable<PostDTO>, IEnumerable<PostViewModel>>(postsDTO);
            foreach (var post in posts)
            {
                post.AuthorName = SetAuthorName(post.UserId);
                post.Comments = postService.CountComments(post.Id);
            }
            return posts;
        }


        private string SetAuthorName(string id)
        {
            userManager = userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            return userManager.FindByIdAsync(id).Result.UserName;
        }
        private ApplicationUser GetUser(string name)
        {
            userManager = userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindByNameAsync(User.Identity.Name);
            return user.Result;
        }
        private string GetPaginationData(int page)
        {
            var paginationMetadata = new
            {
                totalCount = postService.PageInfo.TotalItems,
                pageSize = postService.PageInfo.PageSize,
                currentPage = page,
                totalPages = postService.PageInfo.TotalPages,
                previousPage = page > 1 ? "Yes" : "No",
                nextPage = page < postService.PageInfo.TotalPages ? "Yes" : "No"
            };
            return JsonConvert.SerializeObject(paginationMetadata);
        }
    }
}
