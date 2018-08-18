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
        /// <param name="context"></param> // TODO: ???
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
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UserService() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion

        private User GetUser(string name)
        {
            var user = db.AppUserManager.FindByNameAsync(name);
            return user.Result;
        }
    }
}
