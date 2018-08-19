using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyBlog.BLL.Interfaces;
using MyBlog.Test.Context;
using MyBlog.BLL.Services;
using MyBlog.WEB.Controllers;
using System.Collections.Generic;
using MyBlog.WEB.Models;
using System.Linq;
using System.Web.Http.Results;
using MyBlog.BLL.DTO;
using AutoMapper;
using System.Web.Http;

namespace MyBlog.Test.ApiControllers
{
    [TestClass]
    public class TagsControllerTest
    {
        Mock<ITagService> tagServiceMock;
        TagService tagService;
        UnitOfWork unitOfWork;

        [TestInitialize]
        public void Setup()
        {
            unitOfWork = new UnitOfWork();
            tagService = new TagService(unitOfWork);
            tagServiceMock = new Mock<ITagService>();
        }

        [TestMethod]
        public void Get_GetTopTags_ReturnTags()
        {
            // Arrange
            var controller = new TagsController(tagServiceMock.Object);
            tagServiceMock.Setup(service => service.GetTop()).Returns( Mapper.Map<IEnumerable<TagDTO>>(unitOfWork.TagManager.Get()));

            // Act
            var result = controller.Get() as OkNegotiatedContentResult<IEnumerable<TagViewModel>>;

            // Assert
            Assert.AreEqual(4, result.Content.Count());
        }

        [TestMethod]
        public void Post_AddNewTag_TagShouldBeAdded()
        {
            // Arrange
            var controller = new TagsController(tagService);
            TagViewModel tag = new TagViewModel { Id = 5, Name = "Tag 5" };

            // Act
            IHttpActionResult result = controller.Post(tag);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void Post_AddExistingTag_TagShouldNotBeAdded()
        {
            // Arrange
            var controller = new TagsController(tagService);
            TagViewModel tag = new TagViewModel { Id = 1, Name = "Tag 5" };

            // Act
            IHttpActionResult result = controller.Post(tag);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Post_AddNullTag_TagShouldNotBeAdded()
        {
            // Arrange
            var controller = new TagsController(tagService);
            TagViewModel tag = null;

            // Act
            IHttpActionResult result = controller.Post(tag);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Put_UpdateExisingTag_TagShouldBeUpdated()
        {
            // Arrange
            var controller = new TagsController(tagService);
            TagViewModel tag = new TagViewModel { Id = 1, Name = "Updated Tag 1" };

            // Act
            IHttpActionResult result = controller.Put(tag.Id, tag);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void Put_UpdateNewTag_TagShouldNotBeUpdated()
        {
            // Arrange
            var controller = new TagsController(tagService);
            TagViewModel tag = new TagViewModel { Id = 5, Name = "Tag " };

            // Act
            IHttpActionResult result = controller.Put(tag.Id, tag);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Put_UpdateNullTag_TagShouldNotBeUpdated()
        {
            // Arrange
            var controller = new TagsController(tagService);
            TagViewModel tag = null;

            // Act
            IHttpActionResult result = controller.Put(5, tag);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
