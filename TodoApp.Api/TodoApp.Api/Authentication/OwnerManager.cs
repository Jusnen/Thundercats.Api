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
using TodoApp.Api.Validators;

namespace TodoApp.Api.Authentication
{
    public class OwnerManager : UserManager<Owner>
    {
        public OwnerManager(IUserStore<Owner> store)
            : base(store)
        {

        }

        public static OwnerManager Create(IdentityFactoryOptions<OwnerManager> options, IOwinContext context)
        {
            var appDbContext = context.Get<TodoDbContext>();
            var appUserManager = new OwnerManager(new UserStore<Owner>(appDbContext));


            appUserManager.UserValidator = new MyCustomUserValidator(appUserManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            appUserManager.PasswordValidator = new MyCustomPasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true,
            };

          
            appUserManager.EmailService = new Services.EmailService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
              
                appUserManager.UserTokenProvider = new DataProtectorTokenProvider<Owner>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(6)
                };
            }

       
            return appUserManager;


        }
    }
}
