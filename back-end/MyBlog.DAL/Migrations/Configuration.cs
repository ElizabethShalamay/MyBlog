namespace DAL.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MyBlog.DAL;
    using MyBlog.DAL.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BlogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BlogContext context)
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

            var userStore = new UserStore<User>(context);
            var userManager = new UserManager<User>(userStore);

            CreateUser(userManager, "user1@gmail.com", "user1", "user111", "User");
            CreateUser(userManager, "user2@gmail.com", "user2", "user222", "User");
            CreateUser(userManager, "user3@gmail.com", "user3", "user333", "User");

            CreateUser(userManager, "admin@gmail.com", "admin", "admin123", "Admin");
        

        context.Posts.Add(new Post
            {
                Title = "Дэвид Хэй - о возможном бое Усик - Беллью",
                Description = "Экс-чемпион мира Дэвид Хэй поделился ожиданиями от возможного боя между Александром Усиком и Тони Беллью.",
                Text = "<p>«Усик отличный боец, отличный технарь. Беллью на самом деле не настоящий супертяж. </p>"
                        + "<p>Усик доказал, что он лучший в мире тяжеловес, выиграв турнир.</p>"
                        + "<p>Единственное оставшееся имя в тяжелом весе — это бывший чемпион Тони Беллью», — цитирует Хэя издание vringe.com.</p>",
                UserId = context.Users.Where(u => u.UserName.Equals("user1")).First().Id,
                PostedAt = new DateTime(2018, 7, 31, 7, 28, 0),
                Tags = new List<Tag>{new Tag { Id = 1, Name = "Sport" }, new Tag { Id = 2, Name = "Boxing" } }
            });
            
            context.Posts.Add(new Post
            {
                Title = "5 главных цитат Коко Шанель ",
                Description = "Главные цитаты Коко Шанель, которые нужно знать каждому",
                Text = "<p>1. В двадцать лет женщина имеет лицо, которое дала ей природа, в тридцать — которое она сделала себе сама, в сорок — то, которое она заслуживает.</p>"
                        + "<p>2. Чтобы быть незаменимой, нужно все время меняться.</p>"
                        + "<p>3. Уход за собой должен начаться с сердца, иначе никакая косметика не поможет.</p>"
                        + "<p>4. Кокетство — это победа разума над чувствами.</p>"
                        + "<p>5. Если вы хотите иметь то, что никогда не имели, вам придется делать то, что никогда не делали.</p>",
                UserId = context.Users.Where(u => u.UserName.Equals("user2")).First().Id,
                PostedAt = DateTime.Now,
                Tags = new List<Tag> { new Tag { Id = 3, Name = "Fashion" }, new Tag { Id = 4, Name = "France" } }
            });

            context.Posts.Add(new Post
            {
                Title = "Принципы SOLID",
                Description = "<p>Термин SOLID описывает набор практик проектирования программного кода и построения гибкой и адаптивной программы. <br/>"
                        + "Данный термин был введен 15 лет назад известным американским специалистом в области программирования Робертом Мартином (Robert Martin),<br/>"
                        + " более известным как Uncle Bob (Bob - сокращение от имени Robert)</p>",
                Text = "<p>Single Responsibility Principle (Принцип единственной обязанности)<br/>"
                        + "У класса должна быть только одна причина для изменения.</p>"
                        + "<p>Open/Closed Principle(Принцип открытости / закрытости)<br/>"
                        + "Сущности должны быть открыты для расширения, но закрыты для изменения.</p>"
                        + "<p>Liskov Substitution Principle(Принцип подстановки Лисков)<br/>"
                        + "Объекты в программе могут быть заменены их наследниками без изменения свойств программы.</p>"
                        + "<p>Interface Segregation Principle(Принцип разделения интерфейсов)<br/>"
                        + "Много специализированных интерфейсов лучше, чем один универсальный.</p>"
                        + "<p>Dependency Inversion Principle(Принцип инверсии зависимостей)<br/>"
                        + "Зависимости должны строится относительно абстракций, а не деталей</p>",
                UserId = context.Users.Where(u => u.UserName.Equals("user2")).First().Id,
                PostedAt = new DateTime(2018, 7, 31, 7, 28, 0),
                Tags = new List<Tag> { new Tag { Id = 5, Name = "IT" }}
            });
            context.SaveChanges();
        }

        private void CreateUser(UserManager<User> userManager, string email, string name, string password, string role)
        {
            User user = new User
            {
                Email = email,
                UserName = name
            };

            userManager.Create(user, password);
            userManager.AddToRole(user.Id, role);
        }
    }
}
