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
    // same shit
    [Authorize]
    public class PostsController : ApiController
    {
        IPostService postService;

        public PostsController(IPostService postService)
        {
            this.postService = postService;
        }       

        [Route("api/posts")]
        public IHttpActionResult GetPosts([FromUri] int page = 1)
        {
            IEnumerable<PostDTO> postsDTO = postService.GetPosts(User.Identity.Name, page);
            IEnumerable<PostViewModel> posts = Mapper.Map<IEnumerable<PostDTO>, IEnumerable<PostViewModel>>(postsDTO);

            string pagination_info = postService.GetPaginationData(page);

            return Ok(new { posts = posts, pagination_info = pagination_info });
        }

        [Route("api/posts")]
        public IEnumerable<PostViewModel> GetPosts([FromUri] string text) //  check sharp on frontend side and then call proper server method
        {
            IEnumerable<PostDTO> postsDTO = null;
            text = HttpUtility.UrlDecode(text);
            if (text.StartsWith("#"))
                postsDTO = postService.GetPostsByTag(text);
            else postsDTO = postService.GetPostsByText(text);

            var posts = Mapper.Map<IEnumerable<PostDTO>, IEnumerable<PostViewModel>>(postsDTO);
            
            return posts;
        }

        [Route("api/posts/{id}")]
        public PostViewModel GetPost(int id)
        {
            var postDTO = postService.GetPost(id);
            var post = Mapper.Map<PostDTO, PostViewModel>(postDTO);
            return post;
        }

        [Route("api/posts")]
        public void Post([FromBody] PostViewModel post)
        {
            PostDTO postDTO = Mapper.Map<PostViewModel, PostDTO>(post);
            postDTO.AuthorName = User.Identity.Name;
            postService.AddPost(postDTO, post.Tags);
        }


        [Route("api/posts/{id}")]
        public void Put(int id, [FromBody] PostViewModel post)
        {
            if (id == post.Id)
            {
                var postDTO = Mapper.Map<PostViewModel, PostDTO>(post);
                postService.UpdatePost(postDTO);
            }
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
            
            return posts;
        }
    }
}
