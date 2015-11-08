using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Api.Models;

namespace TodoApp.Api.Db
{
    public class TodoDbContext : IdentityDbContext
    {
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Assistant> Assistants { get; set; }
        public DbSet<Home> Homes { get; set; }

        public TodoDbContext() : base("TodoDb")
        {

        }
        public static TodoDbContext Create()
        {
            return new TodoDbContext();
        }
 
    }
}
