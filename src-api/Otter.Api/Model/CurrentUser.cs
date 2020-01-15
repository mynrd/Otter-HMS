using Microsoft.AspNetCore.Http;
using Otter.Core.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Otter.Api.Model
{
    public class CurrentUser : ICurrentUser
    {
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            Email = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

            IsAuthenticated = UserId != null;
        }

        public string UserId { get; }

        public bool IsAuthenticated { get; }

        public string Email { get; }
    }
}
