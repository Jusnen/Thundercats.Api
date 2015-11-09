using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TodoApp.Api.Authentication;
using Owin;
using Microsoft.AspNet.Identity.Owin;
using TodoApp.Api.Models;
using TodoApp.Api.ViewModels;

namespace TodoApp.Api.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : ApiController
    {
        private ModelFactory _modelFactory;

        protected OwnerManager OwnerManager { get { return Request.GetOwinContext().GetUserManager<OwnerManager>(); } }
        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, this.OwnerManager);
                }
                return _modelFactory;
            }
        }


        [Route("users")]
        [Authorize]
        public IHttpActionResult GetUsers()
        {
            return Ok(this.OwnerManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        [Route("create")]
        public async Task<IHttpActionResult> CreateUser(CreateOwnerModel createOwnerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new Owner()
            {
                UserName = createOwnerModel.Username,
                Email = createOwnerModel.Email,
                FirstName = createOwnerModel.FirstName,
                LastName = createOwnerModel.LastName,
            };

            IdentityResult addUserResult = await this.OwnerManager.CreateAsync(user, createOwnerModel.Password);

            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }

            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

            return Created(locationHeader, TheModelFactory.Create(user));
        }

        [Route("user/{id:guid}", Name = "GetUserById")]
        [Authorize]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            var user = await this.OwnerManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
