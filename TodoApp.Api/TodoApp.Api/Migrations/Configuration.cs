namespace TodoApp.Api.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TodoApp.Api.Db;
    using TodoApp.Api.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TodoApp.Api.Db.TodoDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(TodoApp.Api.Db.TodoDbContext context)
        {

            var manager = new UserManager<Owner>(new UserStore<Owner>(new TodoDbContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new TodoDbContext()));
 

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "Owner" });
                roleManager.Create(new IdentityRole { Name = "Assistant" });
                roleManager.Create(new IdentityRole { Name = "Business" });
            }
 

        }
    }
}
