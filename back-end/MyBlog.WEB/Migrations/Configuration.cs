namespace MyBlog.WEB.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MyBlog.WEB.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            List<IdentityRole> identityRoles = new List<IdentityRole>();
            identityRoles.Add(new IdentityRole() { Name = "Admin" });
            identityRoles.Add(new IdentityRole() { Name = "Guest" });
            identityRoles.Add(new IdentityRole() { Name = "User" });

            foreach (IdentityRole role in identityRoles)
            {
                roleManager.Create(role);
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);            

            CreateUser(userManager, "user1@gmail.com", "user1", "user111", "User");
            CreateUser(userManager, "user2@gmail.com", "user2", "user222", "User");
            CreateUser(userManager, "user3@gmail.com", "user3", "user333", "User");

            CreateUser(userManager, "admin@gmail.com", "admin", "admin123", "Admin");
        }

        private void CreateUser(UserManager<ApplicationUser> userManager, string email, string name, string password, string role)
        {
            ApplicationUser user = new ApplicationUser();
            user.Email = email;
            user.UserName = name;

            userManager.Create(user, password);
            userManager.AddToRole(user.Id, role);
        }
    }
}
