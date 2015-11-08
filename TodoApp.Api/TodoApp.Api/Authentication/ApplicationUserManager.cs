using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Api.Db;
using TodoApp.Api.Models;

namespace TodoApp.Api.Authentication
{
    public class ApplicationUserManager : UserManager<Owner>
    {
        public ApplicationUserManager(IUserStore<Owner> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var appDbContext = context.Get<TodoDbContext>();
            var appUserManager = new ApplicationUserManager(new UserStore<Owner>(appDbContext));

            return appUserManager;
        }
    }
}
