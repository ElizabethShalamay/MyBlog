using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBlog.BLL.DTO;
using MyBlog.BLL.Infrastructure;
using MyBlog.BLL.Interfaces;
using MyBlog.BLL.Services;
using MyBlog.Test.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Test.Services
{
    [TestClass]
    public class CommentServiceTest
    {
        ICommentService service;
        UnitOfWork unitOfWork;

        [TestInitialize]
        public void Setup()
        {
            unitOfWork = new UnitOfWork();
            service = new CommentService(unitOfWork);
        }

        [TestMethod]
        public void GetComment_GetCommentWithId2_ReturnComment()
        {
            // Arrange
            CommentDTO comment = Mapper.Map<CommentDTO>(unitOfWork.CommentManager.Get(2));

            // Act
            CommentDTO result = service.GetComment(2);

            // Assert
            Assert.AreEqual(comment.Id, result.Id);
            Assert.AreEqual(comment.Text, result.Text);
            Assert.AreEqual(comment.AuthorId, result.AuthorId);
            Assert.AreEqual(comment.PostId, result.PostId);
            Assert.AreEqual(comment.ParentId, result.ParentId);
            Assert.AreEqual(comment.IsApproved, result.IsApproved);
            Assert.AreEqual(comment.Date, result.Date);
        }

        [TestMethod]
        public void GetComment_GetCommentWithId10_ReturnNull()
        {
            // Act
            CommentDTO result = service.GetComment(10);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetComments_GetFirstPageOfApprovedComments_Return2Comments()
        {
            // Act
            IEnumerable<CommentDTO> result = service.GetComments(1, 5);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() <= 5);
            Assert.AreEqual(2, result.Count());
        }

        public void GetComments_GetFirstPageOfNonApprovedComments_Return2Comments()
        {
            // Act
            IEnumerable<CommentDTO> result = service.GetComments(1, 5, false);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() <= 5);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetComments_GetThirdPageOfApprovedComments_ReturnNoComments()
        {
            // Act
            IEnumerable<CommentDTO> result = service.GetComments(3, 5, true);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() <= 5);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetComments_GetFirstPageOfCommentsToPost1_ReturnNoComments()
        {
            // Act
            IEnumerable<CommentDTO> result = service.GetComments(1, 1, 5, true);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() <= 5);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetComments_GetFirstPageOfCommentsToPost2_Return1Comments()
        {
            // Act
            IEnumerable<CommentDTO> result = service.GetComments(2, 1, 5, true);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() <= 5);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(1, result.Where(c => c.PostId == 2).FirstOrDefault().Children.Count());
        }

        [TestMethod]
        public void AddComment_AddNewComment_CommentShoudBeAdded()
        {
            // Arrange
            CommentDTO comment = new CommentDTO 
            {
                Id = 5,
                AuthorId = "1",
                ParentId = 0,
                PostId = 1,
                Text = "Comment 5",
                IsApproved = false
            };
            int id = comment.Id;
            int countBefore = unitOfWork.CommentManager.Get().Count();

            // Act
            bool result = service.AddComment(comment, "user1");
            CommentDTO addedComment = Mapper.Map<CommentDTO>(unitOfWork.CommentManager.Get(id));
            int countAfter = unitOfWork.CommentManager.Get().Count();

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(countBefore + 1, countAfter);
            Assert.AreEqual(comment.Id, addedComment.Id);
            Assert.AreEqual(comment.Text, addedComment.Text);
            Assert.AreEqual(comment.AuthorId, addedComment.AuthorId);
            Assert.AreEqual(comment.PostId, addedComment.PostId);
            Assert.AreEqual(comment.ParentId, addedComment.ParentId);
            Assert.AreEqual(comment.IsApproved, addedComment.IsApproved);
        }

        [TestMethod]
        public void AddComment_AddExistingComment_CommentShoudNotBeAdded()
        {
            // Arrange
            CommentDTO comment = new CommentDTO
            {
                Id = 2,
                AuthorId = "1",
                ParentId = 0,
                PostId = 1,
                Text = "Comment 2",
                IsApproved = false
            };
            int id = comment.Id;
            int countBefore = unitOfWork.CommentManager.Get().Count();

            // Act
            bool result = service.AddComment(comment, "user1");
            CommentDTO addedComment = Mapper.Map<CommentDTO>(unitOfWork.CommentManager.Get(id));
            int countAfter = unitOfWork.CommentManager.Get().Count();

            // Assert
            Assert.IsFalse(result);
            Assert.AreNotEqual(comment, addedComment);
            Assert.AreEqual(countBefore, countAfter);
        }

        [TestMethod]
        public void AddComment_AddNullComment_CommentShoudNotBeAdded()
        {
            // Arrange            
            CommentDTO comment = null;
            int countBefore = unitOfWork.CommentManager.Get().Count();

            // Act
            bool result = service.AddComment(comment, "user1");
            int countAfter = unitOfWork.CommentManager.Get().Count();

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(countBefore, countAfter);
        }

        [TestMethod]
        public void DeleteComment_DeleteExistingComment_CommentShoudBeDeleted()
        {
            // Arrange
            int countBefore = unitOfWork.CommentManager.Get().Count();

            // Act
            bool result = service.DeleteComment(3);
            int countAfter = unitOfWork.CommentManager.Get().Count();

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(countBefore - 1, countAfter);
        }

        [TestMethod]
        public void DeleteComment_DeleteNonExistingComment_CommentShoudNotBeDeleted()
        {
            // Arrange
            int countBefore = unitOfWork.CommentManager.Get().Count();
            CommentDTO comment = Mapper.Map<CommentDTO>(unitOfWork.CommentManager.Get(c => c.Id == 5).FirstOrDefault());

            // Act
            bool result = service.DeleteComment(5);
            int countAfter = unitOfWork.CommentManager.Get().Count();

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(comment);
            Assert.AreEqual(countBefore, countAfter);
        }

        [TestMethod]
        public void UpdateComment_UpdateExistingComment_CommentShoudBeUpdated()
        {
            // Arrange            
            CommentDTO newComment = new CommentDTO
            {
                Id = 2,
                AuthorId = "1",
                ParentId = 0,
                PostId = 1,
                Text = "Comment 5",
            };            
            CommentDTO existingComment = Mapper.Map<CommentDTO>(unitOfWork.CommentManager.Get(newComment.Id));

            int countBefore = unitOfWork.CommentManager.Get().Count();

            // Act
            bool result = service.UpdateComment(newComment);
            CommentDTO comment = Mapper.Map<CommentDTO>(unitOfWork.CommentManager.Get(newComment.Id));
            int countAfter = unitOfWork.CommentManager.Get().Count();

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(countBefore, countAfter);
            Assert.AreEqual(comment.Id, newComment.Id);
            Assert.AreEqual(comment.Text, newComment.Text);
            Assert.AreEqual(comment.AuthorId, newComment.AuthorId);
            Assert.AreEqual(comment.PostId, newComment.PostId);
            Assert.AreEqual(comment.ParentId, newComment.ParentId);
            Assert.AreEqual(false, newComment.IsApproved);
        }

        [TestMethod]
        public void UpdateComment_UpdateNonExistingComment_CommentShoudNotBeUpdated()
        {
            // Arrange            
            CommentDTO newComment = new CommentDTO
            {
                Id = 5,
                AuthorId = "1",
                ParentId = 0,
                PostId = 1,
                Text = "Comment 5",
            };
            CommentDTO existingComment = Mapper.Map<CommentDTO>(unitOfWork.CommentManager.Get(newComment.Id));

            int countBefore = unitOfWork.CommentManager.Get().Count();

            // Act
            bool result = service.UpdateComment(newComment);
            CommentDTO comment = Mapper.Map<CommentDTO>(unitOfWork.CommentManager.Get(newComment.Id));
            int countAfter = unitOfWork.CommentManager.Get().Count();

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(countBefore, countAfter);
        }

        [TestMethod]
        public void UpdateComment_UpdateNullComment_CommentShoudNotBeUpdated()
        {
            // Arrange            
            CommentDTO newComment = null;

            int countBefore = unitOfWork.CommentManager.Get().Count();

            // Act
            bool result = service.UpdateComment(newComment);
            int countAfter = unitOfWork.CommentManager.Get().Count();

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(countBefore, countAfter);
        }
    }
}

// TODO: create constants
