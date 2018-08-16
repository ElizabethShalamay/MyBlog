using AutoMapper;
using MyBlog.BLL.DTO;
using MyBlog.BLL.Interfaces;
using MyBlog.DAL.Entities;
using MyBlog.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
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
            var posts= Db.PostManager.Get(p => p.IsApproved == approved)
                                  .OrderByDescending(p => p.PostedAt)
                                  .Skip((page - 1)* PageInfo.PageSize)
                                  .Take(PageInfo.PageSize).ToList();
            return Mapper.Map<IEnumerable<Post>, IEnumerable<PostDTO>>(posts);
        }
        public IEnumerable<PostDTO> GetPosts(string authorId, int pageNum)
        {
            var posts = Db.PostManager.Get().Where(p => p.UserId == authorId && p.IsApproved)
                                  .OrderByDescending(p => p.PostedAt)
                                  .Skip((pageNum-1) * PageInfo.PageSize)
                                  .Take(PageInfo.PageSize).AsEnumerable();
            var postDTOs = Mapper.Map<IEnumerable<Post>, IEnumerable<PostDTO>>(posts);

            foreach(var post in posts)
            {
                var currentPost = postDTOs.Where(p => p.Id == post.Id).First();
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
            var post = Db.PostManager.Get(id);
            if (post == null)
                throw new ValidationException("Пост не найден");
            return Mapper.Map<Post, PostDTO>(post);
        }
        public IEnumerable<PostDTO> GetPostsByTag(string tag)
        {
            tag = tag.Substring(1);
            var tagPosts = Db.TagManager.Get(t => t.Name == tag).ToList();
            var posts = new List<Post>();
            foreach (var item in tagPosts)
            {
                posts.AddRange(item.Posts);
            }
            return Mapper.Map<IEnumerable<Post>, IEnumerable<PostDTO>>(posts);
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

        public void AddPost(PostDTO postDTO, IList<string> tags)
        {
            if (postDTO != null)
            {
                var post = Mapper.Map<PostDTO, Post>(postDTO);
                post.PostedAt = DateTime.Now;
                post.Text = FormatText(post.Text);
                List<Tag> tagList = new List<Tag>();
                foreach(string tag in tags)
                {
                    tagList.Add(new Tag { Name = tag });
                }
                post.Tags = tagList;
                Db.PostManager.Create(post);               
                Db.SaveAsync();
            }
        }
        public void DeletePost(int id)
        {
            Db.PostManager.Delete(id);
            Db.SaveAsync();
        }
        public void UpdatePost(PostDTO postDTO)
        {
            var post = Db.PostManager.Get(postDTO.Id);
            var tags = Db.TagManager.Get().ToList();
            post = Mapper.Map<PostDTO, Post>(postDTO);
            
            post.Tags =tags.Where(tag => tag.Posts.Contains(post)).ToList();
            post.Tags.ToList().RemoveAll(tag => !postDTO.Tags.Contains(tag.Name));
            Db.PostManager.Update(post, post.Id);

            
            foreach(var tag in postDTO.Tags)
            {
                if (Db.TagManager.Get(t => t.Name == tag).FirstOrDefault() == null)
                {
                    var t = new Tag() { Name = tag };
                    post.Tags.Add(t);
                    t.Posts = new List<Post>();
                    t.Posts.Add(Db.PostManager.Get(post.Id));
                    Db.TagManager.Create(t);                  
                }
            }

            Db.SaveAsync();
        }


        private bool Contains(Post post, string text)
        {
            return post.Title.Contains(text) || post.Description.Contains(text) || post.Text.Contains(text);
        }
        private string FormatText(string postText)
        {
            var text = "<p>"+ postText.Replace("\n", "</p><p>") + "</p>";
            text = text.Replace("<p></p>", "");
            return text;
        }
    }
}
