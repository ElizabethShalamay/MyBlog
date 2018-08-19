using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBlog.BLL.DTO;
using MyBlog.BLL.Interfaces;
using MyBlog.BLL.Services;
using MyBlog.Test.Context;
using System.Collections.Generic;
using System.Linq;

namespace MyBlog.Test.Services
{
    [TestClass]
    public class PostServiceTest
    {
        IPostService service;
        UnitOfWork unitOfWork;

        [TestInitialize]
        public void Set()
        {
            unitOfWork = new UnitOfWork();
            service = new PostService(unitOfWork);
        }

        [TestMethod]
        public void GetPost_GetPostWithId2_ReturnPost()
        {
            // Arrange
            PostDTO post = Mapper.Map<PostDTO>(unitOfWork.PostManager.Get(2));

            // Act
            PostDTO result = service.GetPost(2);

            // Assert
            Assert.AreEqual(post.Id, result.Id);
            Assert.AreEqual(post.Text, result.Text);
            Assert.AreEqual(post.UserId, result.UserId);
            Assert.AreEqual(post.IsApproved, result.IsApproved);
            Assert.AreEqual(post.PostedAt, result.PostedAt);
            Assert.AreEqual(post.Title, result.Title);
            Assert.AreEqual(post.Description, result.Description);
            Assert.AreEqual(post.Comments, result.Comments);
            Assert.AreEqual(post.Tags.Count(), result.Tags.Count() );
        }

        [TestMethod]
        public void GetPost_GetPostWithId10_ReturnNull()
        {
            // Act
            PostDTO result = service.GetPost(10);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetPosts_GetFirstPageOfApprovedPosts_Return2Posts()
        {
            // Act
            IEnumerable<PostDTO> result = service.GetPosts(1, true);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() <= 5);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetPosts_GetThisdPageOfApprovedPosts_ReturnNoPosts()
        {
            // Act
            IEnumerable<PostDTO> result = service.GetPosts(3, true);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetPosts_GetFirstPageOfPostsByUser2_Return2Posts()
        {
            // Act
            IEnumerable<PostDTO> result = service.GetPosts("user2", 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() <= 5);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetPosts_GetFirstPageOfPostsByUser3_ReturnNoPosts()
        {
            // Act
            IEnumerable<PostDTO> result = service.GetPosts("user3", 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetPostsByAuthor_GetApprovedPostsByUser2_Return2Posts()
        {
            // Act
            IEnumerable<PostDTO> result = service.GetPostsByAuthor("2");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetPostsByAuthor_GetApprovedPostsByUser10_ReturnNoPosts()
        {
            // Act
            IEnumerable<PostDTO> result = service.GetPostsByAuthor("10");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetPostsByAuthor_GetApprovedPostsByUserNull_ReturnNoPosts()
        {
            // Act
            IEnumerable<PostDTO> result = service.GetPostsByAuthor(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetPostsByTag_GetPostsWithTag1_Return2Posts()
        {
            // Act
            IEnumerable<PostDTO> result = service.GetPostsByTag("Tag 1");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetPostsByTag_GetPostsWithTag5_ReturnNoPosts()
        {
            // Act
            IEnumerable<PostDTO> result = service.GetPostsByTag("Tag 5");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetPostsByTag_GetPostsWithTagNull_ReturnNoPosts()
        {
            // Act
            IEnumerable<PostDTO> result = service.GetPostsByTag(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetPostsByText_GetPostsWithText1_Return1Post()
        {
            // Act
            IEnumerable<PostDTO> result = service.GetPostsByText("Title1");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void GetPostsByText_GetPostsWithDescritpion1_Return1Post()
        {
            // Act
            IEnumerable<PostDTO> result = service.GetPostsByText("Description 1");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void GetPostsByText_GetPostsWithText_Return3Post()
        {
            // Act
            IEnumerable<PostDTO> result = service.GetPostsByText("Text");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void GetPostsByText_GetPostsWithText5_ReturnNoPosts()
        {
            // Act
            IEnumerable<PostDTO> result = service.GetPostsByText("Text 5");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetPostsByText_GetPostsWithTextNull_ReturnNoPosts()
        {
            // Act
            IEnumerable<PostDTO> result = service.GetPostsByText(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void AddPost_AddNewPost_PostShoulBeAdded()
        {
            // Arrange
            PostDTO post = new PostDTO
            {
                Id = 4,
                UserId = "3",
                Title = "Title4",
                Description = "Description 4",
                Text = "Text 4",
                IsApproved = false,
                Tags = new List<string> { "Tag 5", "Tag 2" }
            };
            int id = post.Id;
            int countBefore = unitOfWork.PostManager.Get().Count();

            // Act
            bool result = service.AddPost(post, "user 3", new List<string> { "Tag 5", "Tag 2" });
            PostDTO addedPost = Mapper.Map<PostDTO>(unitOfWork.PostManager.Get(id));
            int countAfter = unitOfWork.PostManager.Get().Count();

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(countBefore + 1, countAfter);
            Assert.AreEqual(post.Id, addedPost.Id);
            Assert.AreEqual(post.Text, addedPost.Text);
            Assert.AreEqual(post.IsApproved, addedPost.IsApproved);
            Assert.AreEqual(post.Title, addedPost.Title);
            Assert.AreEqual(post.Description, addedPost.Description);
            Assert.AreEqual(post.Comments, addedPost.Comments);
            Assert.AreEqual(post.Tags.Count(), addedPost.Tags.Count());
        }

        [TestMethod]
        public void AddPost_AddNullPost_PostShoulNotBeAdded()
        {
            // Arrange
            PostDTO post = null;
            int countBefore = unitOfWork.PostManager.Get().Count();

            // Act
            bool result = service.AddPost(post, "user 3", new List<string> { "Tag 5", "Tag 2" });
            int countAfter = unitOfWork.PostManager.Get().Count();

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(countBefore, countAfter);
        }


        [TestMethod]
        public void DeletePost_DeleteExistingPost_PostShouldBeDeleted()
        {
            // Arrange
            int countBefore = unitOfWork.PostManager.Get().Count();

            // Act
            bool result = service.DeletePost(2);
            int countAfter = unitOfWork.PostManager.Get().Count();

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(countBefore - 1, countAfter);
        }

        [TestMethod]
        public void DeletePost_DeleteNonExistingPost_PostShouldNotBeDeleted()
        {
            // Arrange
            int countBefore = unitOfWork.PostManager.Get().Count();

            // Act
            bool result = service.DeletePost(5);
            int countAfter = unitOfWork.PostManager.Get().Count();

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(countBefore, countAfter);
        }

        [TestMethod]
        public void UpdatePost_UpdateExistingPost_PostShouldBeUpdated()
        {
            // Arrange
            PostDTO post = new PostDTO
            {
                Id = 2,
                UserId = "3",
                Title = "New Title 2",
                Description = "New Description 2",
                Text = "New Text 2",
                Tags = new List<string> { "Tag 5", "Tag 2" }
            };
            int countBefore = unitOfWork.PostManager.Get().Count();
            int id = post.Id;

            // Act
            bool result = service.UpdatePost(post);
            int countAfter = unitOfWork.PostManager.Get().Count();
            PostDTO updatedPost = Mapper.Map<PostDTO>(unitOfWork.PostManager.Get(id));

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(countBefore, countAfter);
            Assert.AreEqual(post.Id, updatedPost.Id);
            Assert.AreEqual(post.Text, updatedPost.Text);
            Assert.AreEqual(false, updatedPost.IsApproved);
            Assert.AreEqual(post.Title, updatedPost.Title);
            Assert.AreEqual(post.Description, updatedPost.Description);
            Assert.AreEqual(post.Comments, updatedPost.Comments);
            Assert.AreEqual(post.Tags.Count(), updatedPost.Tags.Count());
        }

        [TestMethod]
        public void UpdatePost_UpdateNonExistingPost_PostShouldNotBeUpdated()
        {
            // Arrange
            PostDTO post = new PostDTO
            {
                Id = 5,
                UserId = "3",
                Title = "New Title 5",
                Description = "New Description 5",
                Text = "New Text 5",
                Tags = new List<string> { "Tag 5", "Tag 2" }
            };
            int countBefore = unitOfWork.PostManager.Get().Count();
            int id = post.Id;

            // Act
            bool result = service.UpdatePost(post);
            int countAfter = unitOfWork.PostManager.Get().Count();
            PostDTO updatedPost = Mapper.Map<PostDTO>(unitOfWork.PostManager.Get(id));

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(countBefore, countAfter);
            Assert.IsNull(updatedPost);
        }

        [TestMethod]
        public void UpdatePost_UpdateNullPost_PostShouldNotBeUpdated()
        {
            // Arrange
            PostDTO post = null;
            int countBefore = unitOfWork.PostManager.Get().Count();

            // Act
            bool result = service.UpdatePost(post);
            int countAfter = unitOfWork.PostManager.Get().Count();

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(countBefore, countAfter);
        }

        [TestMethod]
        public void GetPaginationData_GetPaginationDataForFirstPage_ReturnJSONString()
        {
            // Act
            string paginationData = service.GetPaginationData(1);

            bool isCorrect = HasCorrectPaginationDataFormat(paginationData);

            // Assert
            Assert.IsNotNull(paginationData);
            Assert.AreNotEqual("", paginationData);
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void GetPaginationData_GetPaginationDataForSecondPage_ReturnJSONString()
        {
            // Act
            service.PageInfo.PageSize = 1;
            string paginationData = service.GetPaginationData(2);

            bool isCorrect = HasCorrectPaginationDataFormat(paginationData);

            // Assert
            Assert.IsNotNull(paginationData);
            Assert.AreNotEqual("", paginationData);
            Assert.IsTrue(isCorrect);
        }

        private bool HasCorrectPaginationDataFormat(string data)
        {
            bool containsTotalCount = data.Contains("\"totalCount\":");
            bool containsPageSize = data.Contains("\"pageSize\":");
            bool containsCurrentPage = data.Contains("\"currentPage\":");
            bool containsTotalPages = data.Contains("\"totalPages\":");
            bool containsPreviousPage = data.Contains("\"previousPage\":");
            bool containsNextPage = data.Contains("\"nextPage\":");

            return containsTotalCount && containsPageSize && containsCurrentPage && containsTotalPages
                && containsPreviousPage && containsNextPage;
        }

        //private bool HasCorrectPaginationDataFormat(string data)
        //{

        //    string correct = "{\"totalCount\":3,\"pageSize\":3,\"currentPage\":1,\"totalPages\":1,\"previousPage\":false,\"nextPage\":false}";
        //    @"\{\"totalCount\"\:\d\,\"currentPage\"\:\d\,\"previousPage\"\:(true|false),\"nextPage\"\:(true|false)\}"
        //}

        //bool containsTotalCount = paginationData.Contains("\"totalCount\":");
        //bool containsPageSize = paginationData.Contains("\"pageSize\":");
        //bool containsCurrentPage = paginationData.Contains("\"currentPage\":");
        //bool containsTotalPages = paginationData.Contains("\"totalPages\":");
        //bool containsPreviousPage = paginationData.Contains("\"previousPage\":");
        //bool containsNextPage = paginationData.Contains("\"nextPage\":");
    }
}

