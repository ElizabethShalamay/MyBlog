using AutoMapper;
using MyBlog.BLL.DTO;
using MyBlog.BLL.Interfaces;
using MyBlog.DAL.Entities;
using MyBlog.DAL.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Owin;
using MyBlog.DAL;
using Microsoft.AspNet.Identity.Owin;
using MyBlog.DAL.Repository;
using System;
using Microsoft.Owin.Security.OAuth;

namespace MyBlog.BLL.Services
{
    /// <summary>
    /// Servise for work with users : reading, adding, removing
    /// </summary>
    public class UserService : IUserService, IDisposable
    {
        IIdentityManager db;
        public UserService(IIdentityManager unitOfWork)
        {
            db = unitOfWork;
        }

        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="userDTO">User data</param>
        /// <param name="password">User password</param>
        /// <returns></returns>
        async Task<IdentityResult> IUserService.CreateUser(UserDTO userDTO, string password)
        {
            User user = Mapper.Map<User>(userDTO);
            try
            {
                user.Id = Guid.NewGuid().ToString();
                return await db.AppUserManager.CreateAsync(user, password);
            }
            catch (Exception ex)
            {

            }

            return await db.AppUserManager.CreateAsync(user, password);
        }        

        /// <summary>
        /// Find an existing user by id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>User with specified id</returns>
        UserDTO IUserService.GetUserById(string id)
        {
            User user = db.AppUserManager.FindByIdAsync(id).Result;
            return Mapper.Map<UserDTO>(user);
        }

        /// <summary>
        /// Find an existing user by name
        /// </summary>
        /// <param name="userName">Name</param>
        /// <returns>User with specified name</returns>
        UserDTO IUserService.GetUserByName(string userName)
        {
            User user = db.AppUserManager.FindByNameAsync(userName).Result;
            return Mapper.Map<UserDTO>(user);
        }

        /// <summary>
        /// Get users per page
        /// </summary>
        /// <param name="page">Page number</param>
        /// <returns></returns>
        IEnumerable<UserDTO> IUserService.GetUsers(int page)
        {
            IEnumerable<User> users = db.AppUserManager.Users.OrderBy(user => user.UserName)
                .Skip((page - 1) * 5).Take(5).ToList();
            return Mapper.Map<IEnumerable<UserDTO>>(users);
        }

       /// <summary>
       /// Create new instance of User Service
       /// </summary>
       /// <returns></returns>
        public static UserService Create()
        {
            return new UserService(new UnitOfWork());
        }

        /// <summary>
        /// Create claims for specific user
        /// </summary>
        /// <param name="context"></param> 
        /// <param name="login">User login</param>
        /// <param name="password">User password</param>
        /// <returns>Claims for user with specified login and password</returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(IOwinContext context, string login, string password)
        {
            var userManager = context.GetUserManager<ApplicationUserManager>();
            User user = await userManager.FindAsync(login, password);

            if (user == null)
            {
                throw new Exception("The user name or password is incorrect.");
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);

            return oAuthIdentity;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        private User GetUser(string name)
        {
            var user = db.AppUserManager.FindByNameAsync(name);
            return user.Result;
        }
    }
}
