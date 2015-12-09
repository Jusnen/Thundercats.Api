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
using System.Configuration;

namespace TodoApp.Api.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {

        [Authorize]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(this.UserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        [AllowAnonymous]
        [Route("owner/create")]
        public async Task<IHttpActionResult> CreateOwner(CreateOwnerModel createOwnerModel)
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

            IdentityResult addUserResult = await this.UserManager.CreateAsync(user, createOwnerModel.Password);

            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }

            this.UserManager.AddToRole(user.Id, "Owner");

            string code = await this.UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

            var callbackurl = Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code });
            var confirmatilPageBaseUrl = ConfigurationManager.AppSettings["email-confirmation-url"];
            var uri = string.Format("{0}#{1}/{2}/{3}", confirmatilPageBaseUrl, "emailconfirmation", Uri.EscapeDataString(user.Id), Uri.EscapeDataString(code));

            await this.UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + uri + "\">here</a>");


            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));


            return Created(locationHeader, TheModelFactory.Create(user));
        }

        [Authorize]
        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            var user = await this.UserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        [Authorize]
        [Route("user/{id:guid}")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {

            //Only SuperAdmin or Admin can delete users (Later when implement roles)

            var appUser = await this.UserManager.FindByIdAsync(id);

            if (appUser != null)
            {
                IdentityResult result = await this.UserManager.DeleteAsync(appUser);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();

            }

            return NotFound();

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.UserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return GetErrorResult(result);
            }
        }


        [Authorize]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }
    }
}
