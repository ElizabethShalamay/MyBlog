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

namespace MyBlog.Test.ApiControllers
{
    [TestClass]
    public class CommentsControllerTest
    {
        Mock<ICommentService> commentServiceMock;
        CommentService commentService;
        UnitOfWork unitOfWork;

        [TestInitialize]
        public void Setup()
        {
            unitOfWork = new UnitOfWork();
            commentService = new CommentService(unitOfWork);
            commentServiceMock = new Mock<ICommentService>();
        }

        [TestMethod]
        public void GetComments_GetCommentsForExistingPost_ReturnComments()
        {
            // Arrange
            var controller = new CommentsController(commentService);

            // Act
            IHttpActionResult result = controller.GetComments(2);

            //Assert
            //Assert.IsInstanceOfType(result, typeof(OkResult));
            Assert.AreEqual(1, (result as OkNegotiatedContentResult<IEnumerable<CommentViewModel>>).Content.Count());
        }

        [TestMethod]
        public void GetComments_GetCommentsForNonExistingPost_ReturnNoComments()
        {
            // Arrange
            var controller = new CommentsController(commentService);

            // Act
            IHttpActionResult result = controller.GetComments(5);

            //Assert
            //Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            Assert.AreEqual(0, (result as OkNegotiatedContentResult<IEnumerable<CommentViewModel>>).Content.Count());
        }

        [TestMethod]
        public void Post_AddNewComment_CommentShouldBeAdded()
        {
            // Arrange
            var controller = new CommentsController(commentService);
            CommentViewModel comment = new CommentViewModel
            {
                Id = 5,
                AuthorId = "1",
                ParentId = 0,
                PostId = 2,
                Text = "Comment 5",
                Date = DateTime.Now,
                IsApproved = false
            };

            // Act
            IHttpActionResult result = controller.PostComment(2, comment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void Post_AddExistingComment_CommentShouldNotBeAdded()
        {
            // Arrange
            var controller = new CommentsController(commentService);
            CommentViewModel comment = new CommentViewModel
            {
                Id = 4,
                AuthorId = "1",
                ParentId = 0,
                PostId = 2,
                Text = "Comment 4",
                Date = DateTime.Now,
                IsApproved = false
            };

            // Act
            IHttpActionResult result = controller.PostComment(2, comment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Post_AddNullComment_CommentShouldNotBeAdded()
        {
            // Arrange
            var controller = new CommentsController(commentService);
            CommentViewModel comment = null;

            // Act
            IHttpActionResult result = controller.PostComment(2, comment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Put_UpdateExistingComment_CommentShouldBeUpdated()
        {
            // Arrange
            var controller = new CommentsController(commentService);
            CommentViewModel comment = new CommentViewModel
            {
                Id = 3,
                AuthorId = "1",
                ParentId = 0,
                PostId = 2,
                Text = "New Comment 3",
                Date = DateTime.Now,
                IsApproved = false
            };

            // Act
            IHttpActionResult result = controller.PutComment(3, comment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void Put_UpdateNewComment_CommentShouldNotBeUpdated()
        {
            // Arrange
            var controller = new CommentsController(commentService);
            CommentViewModel comment = new CommentViewModel
            {
                Id = 5,
                AuthorId = "1",
                ParentId = 0,
                PostId = 2,
                Text = "New Comment 5",
                Date = DateTime.Now,
                IsApproved = false
            };

            // Act
            IHttpActionResult result = controller.PutComment(5, comment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Put_UpdateNullComment_CommentShoulNotdBeUpdated()
        {
            // Arrange
            var controller = new CommentsController(commentService);
            CommentViewModel comment = null;

            // Act
            IHttpActionResult result = controller.PutComment(2, comment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Delete_DeleteExistingComment_CommentShouldBeDeleted()
        {
            // Arrange
            var controller = new CommentsController(commentService);

            // Act
            IHttpActionResult result = controller.DeleteComment(2);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void Delete_DeleteNonExistingComment_CommentShouldNotBeDeleted()
        {
            // Arrange
            var controller = new CommentsController(commentService);

            // Act
            IHttpActionResult result = controller.DeleteComment(5);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
