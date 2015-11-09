using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using TodoApp.Api.Authentication;
using TodoApp.Api.Models;

namespace TodoApp.Api.ViewModels
{
    public class ModelFactory
    {
        private UrlHelper _UrlHelper;
        private OwnerManager _ownerManager;

        public ModelFactory(HttpRequestMessage request, OwnerManager appUserManager)
        {
            _UrlHelper = new UrlHelper(request);
            _ownerManager = appUserManager;
        }

        public UserReturnModel Create(Owner appUser)
        {
            return new UserReturnModel
            {
                Url = _UrlHelper.Link("GetUserById", new { id = appUser.Id }),
                Id = appUser.Id,
                UserName = appUser.UserName,
                FullName = string.Format("{0} {1}", appUser.FirstName, appUser.LastName),
                Email = appUser.Email,
                EmailConfirmed = appUser.EmailConfirmed,
                Roles = _ownerManager.GetRolesAsync(appUser.Id).Result,
                Claims = _ownerManager.GetClaimsAsync(appUser.Id).Result
            };
        }
    }
}
