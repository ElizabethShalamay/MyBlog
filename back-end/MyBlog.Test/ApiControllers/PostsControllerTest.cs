using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyBlog.BLL.Interfaces;
using MyBlog.BLL.Services;
using MyBlog.Test.Context;
using MyBlog.WEB.Controllers;
using System.Web.Http;
using System.Web.Http.Results;
using System.Collections.Generic;
using MyBlog.WEB.Models;
using System.Linq;
using AutoMapper;
using MyBlog.BLL.DTO;
using Newtonsoft.Json;
using MyBlog.BLL;

namespace MyBlog.Test.ApiControllers
{
    [TestClass]
    public class PostsControllerTest
    {
        Mock<IPostService> postServiceMock;
        PostService postService;
        UnitOfWork unitOfWork;

        [TestInitialize]
        public void Setup()
        {
            unitOfWork = new UnitOfWork();
            postService = new PostService(unitOfWork);
            postServiceMock = new Mock<IPostService>();
        }

        [TestMethod]
        public void GetPosts_GetPostsByUser_ReturnPosts() 
        {
            // Arrange
            postServiceMock.Setup(service => service.GetPosts("user2", 1))
                .Returns(Mapper.Map<IEnumerable<PostDTO>>(unitOfWork.PostManager.Get(p => p.Author.UserName == "user2")));
            postServiceMock.Setup(service => service.GetPaginationData(1,"")).Returns("Pagination data");
            postServiceMock.Setup(service => service.PageInfo).Returns(new PageInfo());
            var controller = new PostsController(postServiceMock.Object);

            // Act
            var result = controller.GetPosts(1);

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetNews_GetLatestNews_ReturnPosts() 
        {
            // Arrange
            postServiceMock.Setup(service => service.GetPosts("user2", 1))
                .Returns(Mapper.Map<IEnumerable<PostDTO>>(unitOfWork.PostManager.Get(p => p.Author.UserName == "user2")));
            postServiceMock.Setup(service => service.GetPaginationData(1, "")).Returns("Pagination data");
            postServiceMock.Setup(service => service.PageInfo).Returns(new PageInfo());
            var controller = new PostsController(postServiceMock.Object);

            // Act
            var result = controller.GetNews(1);

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetPost_GetExistingPost_ReturnPost()
        {
            // Arrange
            var controller = new PostsController(postService);

            // Act
            var result = controller.GetPost(1) as OkNegotiatedContentResult<PostViewModel>;

            // Assert
            Assert.AreEqual(1, result.Content.Id);
        }

        [TestMethod]
        public void GetPost_GetNonExistingPost_ReturnNoPost()
        {
            // Arrange
            var controller = new PostsController(postService);

            // Act
            var result = controller.GetPost(5) as OkNegotiatedContentResult<PostViewModel>;

            // Assert
            Assert.IsNull(result.Content);
        }

        [TestMethod]
        public void GetPostsByTag_SearchExistingTag_ReturnPosts()
        {
            // Arrange
            var controller = new PostsController(postService);

            // Act
            var result = controller.GetPostsByTag("Tag 1") as OkNegotiatedContentResult<IEnumerable<PostViewModel>>;

            // Assert
            Assert.AreEqual(2, result.Content.Count());
        }
        [TestMethod]
        public void GetPostsByTag_SearchNonExistingTag_ReturnNoPosts()
        {
            // Arrange
            var controller = new PostsController(postService);

            // Act
            var result = controller.GetPostsByTag("Tag 5") as OkNegotiatedContentResult<IEnumerable<PostViewModel>>;

            // Assert
            Assert.AreEqual(0, result.Content.Count());
        }

        [TestMethod]
        public void GetPostsByText_SearchExistingText_ReturnPosts()
        {
            // Arrange
            var controller = new PostsController(postService);

            // Act
            var result = controller.GetPostsByText("Text") as OkNegotiatedContentResult<IEnumerable<PostViewModel>>;

            // Assert
            Assert.AreEqual(3, result.Content.Count());
        }

        [TestMethod]
        public void GetPostsByText_SearchNonExistingText_ReturnNoPosts()
        {
            // Arrange
            var controller = new PostsController(postService);

            // Act
            var result = controller.GetPostsByText("Error") as OkNegotiatedContentResult<IEnumerable<PostViewModel>>;

            // Assert
            Assert.AreEqual(0, result.Content.Count());
        }

        [TestMethod]
        public void Post_AddNewPost_PostShouldBeAdded()
        {
            // Arrange
            var controller = new PostsController(postService);
            PostViewModel post = new PostViewModel
            {
                Id = 5,
                UserId = "1",
                AuthorName = "user1",
                Title = "Title5",
                Description = "Description 5",
                Text = "Post 5",
                PostedAt = DateTime.Now,
                IsApproved = false,
                Tags = new List<string> { "Tag 1"}
            };

            // Act
            IHttpActionResult result = controller.Post(post);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void Post_AddExistingPost_PostShouldNotBeAdded()
        {
            // Arrange
            var controller = new PostsController(postService);
            PostViewModel post = new PostViewModel
            {
                Id = 3,
                UserId = "3",
                AuthorName = "user3",
                Title = "Title3",
                Description = "Description 3",
                Text = "Post 3",
                PostedAt = DateTime.Now,
                IsApproved = false,
                Tags = new List<string> { "Tag 1" }
            };

            // Act
            IHttpActionResult result = controller.Post(post);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Post_AddNullPost_PostShouldNotBeAdded()
        {
            // Arrange
            var controller = new PostsController(postService);
            PostViewModel post = null;

            // Act
            IHttpActionResult result = controller.Post(post);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Put_UpdateExistingPost_PostShouldBeUpdated()
        {
            // Arrange
            var controller = new PostsController(postService);

            PostViewModel post = new PostViewModel
            {
                Id = 2,
                UserId = "1",
                AuthorName = "user1",
                Title = "Title2",
                Description = "Description 2",
                Text = "New Post 2",
                PostedAt = DateTime.Now,
                IsApproved = false
            };

            // Act
            IHttpActionResult result = controller.Put(2, post);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void Put_UpdateNewPost_PostShouldNotBeUpdated()
        {
            // Arrange
            var controller = new PostsController(postService);
            PostViewModel post = new PostViewModel
            {
                Id = 5,
                UserId = "1",
                AuthorName = "user1",
                Title = "Title5",
                Description = "Description 5",
                Text = "New Post 5",
                PostedAt = DateTime.Now,
                IsApproved = false
            };

            // Act
            IHttpActionResult result = controller.Put(5, post);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Put_UpdateNullPost_PostShoulNotdBeUpdated()
        {
            // Arrange
            var controller = new PostsController(postService);
            PostViewModel post = null;

            // Act
            IHttpActionResult result = controller.Put(2, post);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Delete_DeleteExistingPost_PostShouldBeDeleted()
        {
            // Arrange
            var controller = new PostsController(postService);

            // Act
            IHttpActionResult result = controller.Delete(2);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void Delete_DeleteNonExistingPost_PostShouldNotBeDeleted()
        {
            // Arrange
            var controller = new PostsController(postService);

            // Act
            IHttpActionResult result = controller.Delete(5);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
