using System;
using System.Collections.Generic;
using MyBlog.DAL;
using MyBlog.DAL.Entities;
using MyBlog.DAL.Interfaces;

namespace MyBlog.Test.Context
{
    class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork()
        {
            var tag1 = new Tag { Id = 1, Name = "Tag 1" };
            var tag2 = new Tag { Id = 2, Name = "Tag 2" };
            var tag3 = new Tag { Id = 3, Name = "Tag 3" };
            var tag4 = new Tag { Id = 4, Name = "Tag 4" };

            var post1 = new Post { Id = 1, UserId = "1", Title = "Title1",
                Description = "Description 1", Text = "Text 1", PostedAt = new DateTime(2017, 2, 3),
                IsApproved = false, Tags = new List<Tag> { tag1, tag2 } };
            var post2 = new Post { Id = 2, UserId = "2", Title = "Title2",
                Description = "Description 2", Text = "Text 2", PostedAt = new DateTime(2016, 3, 22),
                IsApproved = true, Tags = new List<Tag> { tag1, tag3 } };
            var post3 = new Post { Id = 3, UserId = "2", Title = "Title3",
                Description = "Description 3", Text = "Text 3", PostedAt = new DateTime(2015, 6, 14),
                IsApproved = true, Tags = new List<Tag> { tag2, tag4 } };

            tag1.Posts = new List<Post> { post1, post2 };
            tag2.Posts = new List<Post> { post1, post2 };
            tag3.Posts = new List<Post> { post2 };
            tag4.Posts = new List<Post> { post3 };

            var comment1 = new Comment { Id = 1, AuthorId = "1", ParentId = 0, PostId = 2,
                Text = "Comment 1", Date = new DateTime(2016, 6, 5), IsApproved = true };
            var comment2 = new Comment { Id = 2, AuthorId = "2", ParentId = 1, PostId = 2,
                Text = "Comment 2", Date = new DateTime(2016, 6, 13), IsApproved = true };
            var comment3 = new Comment { Id = 3, AuthorId = "2", ParentId = 0, PostId = 3,
                Text = "Comment 3", Date = new DateTime(2017, 2, 12), IsApproved = false };
            var comment4 = new Comment { Id = 4, AuthorId = "1", ParentId = 0, PostId = 3,
                Text = "Comment 4", Date = new DateTime(2016, 6, 5), IsApproved = false };

            List<Tag> tags = new List<Tag> { tag1, tag2, tag3, tag4 };
            List<Post> posts = new List<Post> { post1, post2, post3 };
            List<Comment> comments = new List<Comment> { comment1, comment2, comment3, comment4 };

            PostManager = new PostRepository(posts);
            CommentManager = new CommentRepository(comments);
            TagManager = new TagRepository(tags);
        }

        public IRepository<Post> PostManager { get; set; }

        public IRepository<Comment> CommentManager { get; set; }

        public IRepository<Tag> TagManager { get; set; }

        public BlogContext Blog { get; set; }


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
