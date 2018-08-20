using AutoMapper;
using MyBlog.BLL.DTO;
using MyBlog.BLL.Interfaces;
using MyBlog.DAL.Entities;
using MyBlog.DAL.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.BLL.Services
{
    /// <summary>
    /// Service for work with posts : reading, adding, editing, removing
    /// </summary>
    public class PostService : IPostService
    {
        const int POST_PAGE_SIZE = 2;
        const int NEWS_PAGE_SIZE = 4;
        const int ADMIN_PAGE_SIZE = 15;

        IUnitOfWork Db { get; set; }
        public PageInfo PageInfo { get; set; }

        public PostService(IUnitOfWork unitOfWork)
        {
            Db = unitOfWork;
            PageInfo = new PageInfo();
        }

        /// <summary>
        /// Return list of posts for current page
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="approved">If post should be approved to be chosen</param>
        /// <returns>List of posts for current page</returns>
        IEnumerable<PostDTO> IPostService.GetPosts(int page, bool approved)
        {
            List<Post> posts = Db.PostManager.Get(p => p.IsApproved == approved)
                                  .OrderByDescending(p => p.PostedAt)
                                  .Skip((page - 1) * PageInfo.PageSize)
                                  .Take(PageInfo.PageSize).ToList();
            IEnumerable<PostDTO> postsDTO = Mapper.Map<IEnumerable<Post>, IEnumerable<PostDTO>>(posts);
            foreach (PostDTO post in postsDTO)
            {
                post.AuthorName = SetAuthorName(post.UserId);
                post.Comments = CountComments(post.Id);
            }
            return postsDTO;
        }

        /// <summary>
        /// Return list of posts of specific author for current page
        /// </summary>
        /// <param name="authorName">Author name</param>
        /// <param name="pageNum">Page number</param>
        /// <returns>List of posts of specific author for current page</returns>
        IEnumerable<PostDTO> IPostService.GetPosts(string authorName, int pageNum)
        {
            IEnumerable<Post> posts = Db.PostManager.Get().ToList().Where(p => p.Author.UserName == authorName && p.IsApproved)
                                  .OrderByDescending(p => p.PostedAt)
                                  .Skip((pageNum - 1) * PageInfo.PageSize)
                                  .Take(PageInfo.PageSize).ToList();
            IEnumerable<PostDTO> postDTOs = Mapper.Map<IEnumerable<PostDTO>>(posts);

            foreach (PostDTO post in postDTOs)
            {
                post.AuthorName = SetAuthorName(post.UserId);
            }
            foreach (Post post in posts)
            {
                PostDTO currentPost = postDTOs.Where(p => p.Id == post.Id).First();
                currentPost.Tags = post.Tags.Select(t => t.Name);
            }
            return postDTOs;
        }

        /// <summary>
        /// Return count of comments for specific post
        /// </summary>
        /// <param name="postId">Post id</param>
        /// <returns>Count of comments for specific post</returns>
        public int CountComments(int postId)
        {
            return Db.CommentManager.Get(c => c.PostId == postId && c.IsApproved).Count();
        }

        /// <summary>
        /// Return post with specific id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Post with specific id</returns>
        PostDTO IPostService.GetPost(int id)
        {
            Post post = Db.PostManager.Get(id);
            if (post == null)
                return null;
            PostDTO postDTO = Mapper.Map<Post, PostDTO>(post);
            postDTO.AuthorName = SetAuthorName(post.UserId);
            return postDTO;
        }

        /// <summary>
        /// Search for posts with specific tag
        /// </summary>
        /// <param name="tag">Tag</param>
        /// <returns>List of posts with specific tag</returns>
        IEnumerable<PostDTO> IPostService.GetPostsByTag(string tag)
        {
            List<Tag> tagPosts = Db.TagManager.Get(t => t.Name.Equals(tag, StringComparison.OrdinalIgnoreCase)).ToList();

            List<Post> posts = tagPosts.SelectMany(item => item.Posts).ToList();
            IEnumerable<PostDTO> postDTOs = Mapper.Map<IEnumerable<PostDTO>>(posts);

            foreach (PostDTO post in postDTOs)
            {
                post.AuthorName = SetAuthorName(post.UserId);
            }

            return postDTOs;
        }

        /// <summary>
        /// Search for posts containing specific text
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>List of posts containing specific text</returns>
        IEnumerable<PostDTO> IPostService.GetPostsByText(string text)
        {
            if (text == null)
                return new List<PostDTO>();
            IEnumerable<Post> posts = Db.PostManager.Get(p => Contains(p, text)).ToList();
            IEnumerable<PostDTO> postDTOs = Mapper.Map<IEnumerable<PostDTO>>(posts);

            foreach (PostDTO post in postDTOs)
            {
                post.AuthorName = SetAuthorName(post.UserId);
            }
            return postDTOs;
        }

        /// <summary>
        /// Return list of posts created by specific user
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>List of posts created by specific user</returns>
        IEnumerable<PostDTO> IPostService.GetPostsByAuthor(string id)
        {
            IEnumerable<Post> posts = Db.PostManager.Get(p => p.UserId.Equals(id)).ToList();
            return Mapper.Map<IEnumerable<Post>, IEnumerable<PostDTO>>(posts);
        }

        /// <summary>
        /// Add a new post
        /// </summary>
        /// <param name="postDTO">Post</param>
        /// <param name="userName">Author name</param>
        /// <param name="tags">List of tags</param>
        /// <returns>Boolean value of operation success</returns>
        bool IPostService.AddPost(PostDTO postDTO, string userName, IList<string> tags)
        {
            if (postDTO == null)
                return false;

            Post post = Mapper.Map<Post>(postDTO);
            post.UserId = GetUser(userName);
            post.PostedAt = DateTime.Now;
            // post.Text = FormatText(post.Text); // TODO: Add formating on front-end

            List<Tag> tagList = new List<Tag>();
            foreach (string tag in tags)
            {
                tagList.Add(new Tag { Name = tag });
            }

            post.Tags = tagList;

            int result = Db.PostManager.Create(post);
            return result > 0;
        }

        /// <summary>
        /// Remove post with soecific id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Boolean value of operation success</returns>
        bool IPostService.DeletePost(int id)
        {
            int result = Db.PostManager.Delete(id);
            return result > 0;
        }

        /// <summary>
        /// Edit an existing post
        /// </summary>
        /// <param name="postDTO">Post</param>
        /// <returns>Boolean value of operation success</returns>
        bool IPostService.UpdatePost(PostDTO postDTO)
        {
            if (postDTO == null)
                return false;

            Post post = Db.PostManager.Get(postDTO.Id);
            Post oldPost = Db.PostManager.Get(postDTO.Id);
            if (post == null)
                return false;

            List<Tag> tags = Db.TagManager.Get().ToList();
            post = Mapper.Map<PostDTO, Post>(postDTO);

            post.Tags = tags.Where(tag => tag.Posts.Contains(oldPost) && postDTO.Tags.Contains(tag.Name)).ToList();
            int result = Db.PostManager.Update(post);

            foreach (string tag in postDTO.Tags)
            {
                if (!Db.TagManager.Get(t => t.Name == tag).Any())
                {
                    Tag t = new Tag { Name = tag };
                    post.Tags.Add(t);
                    t.Posts = new List<Post> { Db.PostManager.Get(post.Id) };
                    Db.TagManager.Create(t);
                }
            }

            return result > 0;
        }

        /// <summary>
        /// Create pagination information
        /// </summary>
        /// <param name="page">Page number</param>
        /// <returns>JSON string with pagination info</returns>
        string IPostService.GetPaginationData(int page, string userName)
        {
            string id = "";
            if (userName != "")
                id = (Db as IIdentityManager).AppUserManager.FindByNameAsync(userName).Result.Id;

            PageInfo.TotalItems = PageInfo.PageSize == POST_PAGE_SIZE ? Db.PostManager.Get(p => p.UserId == id).Count() :
                Db.PostManager.Get().Count();
            var paginationMetadata = new
            {
                totalCount = PageInfo.TotalItems,
                pageSize = PageInfo.PageSize,
                currentPage = page,
                totalPages = PageInfo.TotalPages,
                previousPage = page > 1 ? true : false,
                nextPage = page < PageInfo.TotalPages ? true : false
            };
            return JsonConvert.SerializeObject(paginationMetadata);
        }

        private bool Contains(Post post, string text)
        {
            string lower = text.ToLower();
            return post.Title.ToLower().Contains(lower) 
                || post.Description.ToLower().Contains(lower) 
                || post.Text.ToLower().Contains(lower);
        }

        //private string FormatText(string postText) // check format in frontend
        //{
        //    return "<p>" + postText.Replace("\n", "</p><p>").Replace("<p></p>", "") + "</p>";
        //}

        private string SetAuthorName(string id)
        {
            if (Db as IIdentityManager == null)
                return null;
            return (Db as IIdentityManager).AppUserManager.FindByIdAsync(id).Result.UserName;
        }

        private string GetUser(string name)
        {
            if (Db as IIdentityManager == null)
                return null;

            Task<User> user = (Db as IIdentityManager).AppUserManager.FindByNameAsync(name);
            return user.Result.Id;
        }
    }
}
