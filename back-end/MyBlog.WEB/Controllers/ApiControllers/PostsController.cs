using AutoMapper;
using MyBlog.BLL.DTO;
using MyBlog.BLL.Interfaces;
using MyBlog.WEB.Models;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace MyBlog.WEB.Controllers
{
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

        [Route("api/search/tag")]
        public IHttpActionResult GetPostsByTag([FromUri] string tag)
        {
            IEnumerable<PostDTO> postsDTO = postService.GetPostsByTag(HttpUtility.UrlDecode(tag));

            IEnumerable<PostViewModel> posts = Mapper.Map<IEnumerable<PostViewModel>>(postsDTO);

            return Ok(posts);
        }

        [Route("api/search/text")]
        public IHttpActionResult GetPostsByText([FromUri] string text)
        {
            IEnumerable<PostDTO> postsDTO = postService.GetPostsByText(HttpUtility.UrlDecode(text));

            IEnumerable<PostViewModel> posts = Mapper.Map<IEnumerable<PostViewModel>>(postsDTO);

            return Ok(posts);
        }

        [Route("api/posts/{id}")]
        public IHttpActionResult GetPost(int id)
        {
            PostDTO postDTO = postService.GetPost(id);
            PostViewModel post = Mapper.Map<PostViewModel>(postDTO);

            return Ok(post);
        }

        [Route("api/posts")]
        public IHttpActionResult Post([FromBody] PostViewModel post)
        {
            if (post == null)
                return BadRequest();

            PostDTO postDTO = Mapper.Map<PostDTO>(post);

            bool success = postService.AddPost(postDTO, User.Identity.Name, post.Tags);

            if (success)
                return Ok();
            return BadRequest();
        }


        [Route("api/posts/{id}")]
        public IHttpActionResult Put(int id, [FromBody] PostViewModel post)
        {
            if (post == null || id != post.Id)
                return BadRequest();

            PostDTO postDTO = Mapper.Map<PostDTO>(post);
            bool success = postService.UpdatePost(postDTO);

            if (success)
                return Ok();

            return BadRequest();
        }

        [Route("api/posts/{id}")]
        public IHttpActionResult Delete(int id)
        {
            bool success = postService.DeletePost(id);

            if (success)
                return Ok();
            return BadRequest();
        }

        [Route("api/posts/news")]
        public IHttpActionResult GetNews([FromUri] int page = 1)
        {
            IEnumerable<PostDTO> postsDTO = postService.GetPosts(page, true);
            IEnumerable<PostViewModel> posts = Mapper.Map<IEnumerable<PostViewModel>>(postsDTO);

            return Ok(posts);
        }
    }
}
