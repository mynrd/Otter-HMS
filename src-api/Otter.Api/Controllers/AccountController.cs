using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Otter.Core.Account;
using Otter.Services.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otter.Api.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserAccessService _userService;

        public AccountController(IUserAccessService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Login - Required Fields
        ///     Company
        ///     SiteCode
        ///     Username
        ///     Password
        /// </summary>
        /// <param name="login">Login Model</param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public IActionResult Authenticate([FromBody]LoginModel login)
        {
            var user = _userService.Authenticate(login);

            return user.Status switch
            {
                LoginStatus.Success => Ok(new
                {
                    success = true,
                    user
                }),
                _ => BadRequest(new
                {
                    success = false,
                    user
                }),
            };
        }
    }
}