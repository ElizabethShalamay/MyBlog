using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBlog.BLL.DTO;
using MyBlog.BLL.Infrastructure;
using MyBlog.BLL.Interfaces;
using MyBlog.BLL.Services;
using MyBlog.Test.Context;
using System.Collections.Generic;
using System.Linq;

namespace MyBlog.Test.Services
{
    [TestClass]
    public class TagServiceTest
    {
        ITagService service;
        UnitOfWork unitOfWork;

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            var config = WEB.App_Start.MappingConfig.InitializeMapper();
            MappingConfig.InitializeMapper(config);
        }

        [TestInitialize]
        public void Setup()
        {
            unitOfWork = new UnitOfWork();
            service = new TagService(unitOfWork);
        }

        [TestMethod]
        public void GetAll_GetAllTags_TagsCountShouldBeEqual()
        {
            // Act
            IEnumerable<TagDTO> tags = service.GetAll();

            // Assert
            Assert.IsTrue(tags.Count() == unitOfWork.TagManager.Get().Count());
            Assert.AreEqual(4, tags.Count());
        }

        [TestMethod]
        public void GetTop_GetTop20Tags_TagsShouldBeEqual()
        {
            // Act
            IEnumerable<TagDTO> tags = service.GetTop();

            //Assert
            Assert.IsTrue(tags.Count() < 20);
            Assert.AreEqual(4, tags.Count());
        }

        [TestMethod]
        public void AddTag_AddNewTag_TagsShoudBeEqual()
        {
            // Arrange
            TagDTO tag = new TagDTO { Id = 5, Name = "Tag 5" };
            int id = tag.Id;

            // Act
            bool result = service.AddTag(tag);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(tag.Id, unitOfWork.TagManager.Get(id).Id);
            Assert.AreEqual(tag.Name, unitOfWork.TagManager.Get(tag.Id).Name);
        }

        [TestMethod]
        public void AddTag_AddExistingTag_TagShoudNotBeAdded()
        {
            // Arrange
            int countBefore = service.GetAll().Count();
            TagDTO newTag = new TagDTO { Id = 3, Name = "Tag 5" };

            // Act
            bool result = service.AddTag(newTag);
            int countAfter = service.GetAll().Count();

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(countBefore, countAfter);
            Assert.AreNotEqual(newTag, Mapper.Map<TagDTO>(unitOfWork.TagManager.Get(3)));
        }
        public void AddTag_AddExistingTagName_TagShoudNotBeAdded()
        {
            // Arrange
            int countBefore = service.GetAll().Count();
            TagDTO newTag = new TagDTO { Id = 5, Name = "Tag 3" };

            // Act
            bool result = service.AddTag(newTag);
            int countAfter = service.GetAll().Count();

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(countBefore, countAfter);
        }

        [TestMethod]
        public void AddTag_AddNullTag_TagShoudNotBeAdded()
        {
            // Arrange            
            TagDTO newTag = null;

            // Act
            bool result = service.AddTag(newTag);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdateTag_UpdateExistingTag_TagShoudBeUpdated()
        {
            // Arrange            
            TagDTO newTag = new TagDTO { Id = 3, Name = "Tag 5" };
            TagDTO existingTag = Mapper.Map<TagDTO>(unitOfWork.TagManager.Get(newTag.Id));

            // Act
            bool result = service.UpdateTag(newTag);

            TagDTO tag = Mapper.Map<TagDTO>(unitOfWork.TagManager.Get(newTag.Id));

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(newTag.Id, tag.Id);
            Assert.AreEqual(newTag.Name, tag.Name);
            Assert.AreNotEqual(existingTag, newTag);
        }

        [TestMethod]
        public void UpdateTag_UpdateNonExistingTag_TagShoudNotBeUpdated()
        {
            // Arrange            
            TagDTO newTag = new TagDTO { Id = 6, Name = "Tag 6" };

            // Act
            bool result = service.UpdateTag(newTag);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdateTag_UpdateNullTag_TagShoudNotBeUpdated()
        {
            // Arrange            
            TagDTO newTag = null;

            // Act
            bool result = service.UpdateTag(newTag);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
