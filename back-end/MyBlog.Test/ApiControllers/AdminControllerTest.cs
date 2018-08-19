using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBlog.Test.Context;
using MyBlog.BLL.Services;
using Moq;
using MyBlog.BLL.Interfaces;
using System.Collections.Generic;
using MyBlog.BLL.DTO;
using MyBlog.WEB.Controllers;
using System.Web.Http.Results;
using MyBlog.WEB.Models;
using System.Linq;

namespace MyBlog.Test.ApiControllers
{
    [TestClass]
    public class AdminControllerTest
    {
        UnitOfWork unitOfWork;
        PostService postService;
        CommentService commentService;
        Mock<IUserService> userServiceMock;

        [TestInitialize]
        public void Setup()
        {
            unitOfWork = new UnitOfWork();

            postService = new PostService(unitOfWork);
            commentService = new CommentService(unitOfWork);
            userServiceMock = new Mock<IUserService>();
        }

        [TestMethod]
        public void GetUsers_ReturnUsers()
        {
            // Arrange
            userServiceMock.Setup(service => service.GetUsers(1)).Returns(new List<UserDTO>
            {
                new UserDTO {Id = "1", UserName = "user1" },
                new UserDTO {Id = "2", UserName = "user2" },
                new UserDTO {Id = "3", UserName = "user3" }
            });

            var controller = new AdminController(postService, commentService, userServiceMock.Object);

            // Act
            var result = controller.GetUsers(1) as OkNegotiatedContentResult<IEnumerable<UserViewModel>>;

            // Assert
            Assert.AreEqual(3, result.Content.Count());
        }

        [TestMethod]
        public void GetUser_ReturnUser()
        {
            // Arrange
            userServiceMock.Setup(service => service.GetUserById("1")).Returns(new UserDTO
            { Id = "1", UserName = "user1", Email = "email1@gmail.com", PasswordHash = "hash123" });

            var controller = new AdminController(postService, commentService, userServiceMock.Object);

            // Act
            var result = controller.GetUser("1") as OkNegotiatedContentResult<UserViewModel>;

            // Assert
            Assert.AreEqual("1", result.Content.Id);
            Assert.AreEqual("user1", result.Content.UserName);
        }

        [TestMethod]
        public void GetPosts_ReturnPosts()
        {
            // Arrange
            var controller = new AdminController(postService, commentService, userServiceMock.Object);

            // Act
            var result = controller.GetPosts(1) as OkNegotiatedContentResult<IEnumerable<PostViewModel>>;

            // Assert
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetPost_GetExistingPost_ReturnPost()
        {
            // Arrange
            var controller = new AdminController(postService, commentService, userServiceMock.Object);

            // Act
            var result = controller.GetPost(1) as OkNegotiatedContentResult<PostViewModel>;

            // Assert
            Assert.AreEqual(1, result.Content.Id);
            Assert.AreEqual("Title1", result.Content.Title);
        }

        [TestMethod]
        public void GetPost_GetNonExistingPost_ReturnNoPost()
        {
            // Arrange
            var controller = new AdminController(postService, commentService, userServiceMock.Object);

            // Act
            var result = controller.GetPost(5) as OkNegotiatedContentResult<PostViewModel>;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetComments_ReturnComments()
        {
            // Arrange
            var controller = new AdminController(postService, commentService, userServiceMock.Object);

            // Act
            var result = controller.GetComments(1) as OkNegotiatedContentResult<IEnumerable<CommentViewModel>>;

            // Assert
            Assert.AreEqual(2, result.Content.Count());
        }

        [TestMethod]
        public void GetComment_GetExistingComment_ReturnComment()
        {
            // Arrange
            var controller = new AdminController(postService, commentService, userServiceMock.Object);

            // Act
            var result = controller.GetComment(1) as OkNegotiatedContentResult<CommentViewModel>;

            // Assert
            Assert.AreEqual(1, result.Content.Id);
        }

        [TestMethod]
        public void GetComment_GetNonExistingComment_ReturnNoComment()
        {
            // Arrange
            var controller = new AdminController(postService, commentService, userServiceMock.Object);

            // Act
            var result = controller.GetComment(5) as OkNegotiatedContentResult<CommentViewModel>;

            // Assert
            Assert.IsNull(result);
        }
    }
}
