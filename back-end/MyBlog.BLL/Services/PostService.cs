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
    public class PostService : IPostService
    {
        //TODO: write method GetTotalPages(); 
        // get data from BlogSettings 

        IUnitOfWork Db { get; set; }
        public PageInfo PageInfo { get; set; }

        public PostService(IUnitOfWork unitOfWork)
        {
            Db = unitOfWork;
            PageInfo = new PageInfo { PageNumber = 1, PageSize = 3, TotalItems = Db.PostManager.Get().Count() };
        }

        public IEnumerable<PostDTO> GetPosts(int page, bool approved)
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
        public IEnumerable<PostDTO> GetPosts(string authorName, int pageNum)
        {
            IEnumerable < Post> posts = Db.PostManager.Get().ToList().Where(p => p.Author.UserName == authorName && p.IsApproved)
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

        public int CountComments(int postId)
        {
            return Db.CommentManager.Get(c => c.PostId == postId).Count();
        }
        public PostDTO GetPost(int id)
        {
            Post post = Db.PostManager.Get(id);
            if (post == null)
                throw new ValidationException("Пост не найден");
            PostDTO postDTO = Mapper.Map<Post, PostDTO>(post);
            postDTO.AuthorName = SetAuthorName(post.UserId);
            return postDTO;
        }
        public IEnumerable<PostDTO> GetPostsByTag(string tag)
        {
            List<Tag> tagPosts = Db.TagManager.Get(t => t.Name == tag).ToList();

            List<Post> posts = tagPosts.SelectMany(item => item.Posts).ToList();
            IEnumerable<PostDTO> postDTOs = Mapper.Map<IEnumerable<PostDTO>>(posts);

            foreach (PostDTO post in postDTOs)
            {
                post.AuthorName = SetAuthorName(post.UserId);
            }

            return postDTOs;
        }
        public IEnumerable<PostDTO> GetPostsByText(string text)
        {
            IEnumerable<Post> posts = Db.PostManager.Get(p => Contains(p, text));
            return Mapper.Map<IEnumerable<Post>, IEnumerable<PostDTO>>(posts);
        }
        public IEnumerable<PostDTO> GetPostsByAuthor(string id)
        {
            IEnumerable<Post> posts = Db.PostManager.Get(p => p.UserId.Equals(id));
            return Mapper.Map<IEnumerable<Post>, IEnumerable<PostDTO>>(posts);
        }

        public bool AddPost(PostDTO postDTO, string userName, IList<string> tags)
        {
            if (postDTO == null)
                return false;

            Post post = Mapper.Map<Post>(postDTO);
            post.UserId = GetUser(userName).Id;
            post.PostedAt = DateTime.Now;
            post.Text = FormatText(post.Text);

            List<Tag> tagList = new List<Tag>();//.SelectMany(item => tags);
            foreach (string tag in tags) // select many
            {
                tagList.Add(new Tag { Name = tag });
            }

            post.Tags = tagList;

            int result = Db.PostManager.Create(post);
            return result > 0;
        }
        public bool DeletePost(int id)
        {
            int result = Db.PostManager.Delete(id);
            return result > 0;
        }
        public bool UpdatePost(PostDTO postDTO)
        {
            if (postDTO == null)
                return false;

            Post post = Db.PostManager.Get(postDTO.Id);
            List<Tag> tags = Db.TagManager.Get().ToList();
            post = Mapper.Map<PostDTO, Post>(postDTO);

            post.Tags = tags.Where(tag => tag.Posts.Contains(post)).ToList();
            post.Tags.ToList().RemoveAll(tag => !postDTO.Tags.Contains(tag.Name));
            int result = Db.PostManager.Update(post, post.Id);

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

        private bool Contains(Post post, string text)
        {
            return post.Title.Contains(text) || post.Description.Contains(text) || post.Text.Contains(text);
        }

        string IPostService.GetPaginationData(int page)
        {
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

        private string FormatText(string postText) // check format in frontend
        {
            return "<p>" + postText.Replace("\n", "</p><p>").Replace("<p></p>", "") + "</p>";
        }

        private string SetAuthorName(string id)
        {         
            return Db.AppUserManager.FindByIdAsync(id).Result.UserName;
        }

        private User GetUser(string name)
        {
            Task<User> user = Db.AppUserManager.FindByNameAsync(name);
            return user.Result;
        }
    }
}
