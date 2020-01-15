using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Otter.Core;
using Otter.Core.Constant;
using Otter.Services.Account;
using System.Linq;

namespace Otter.Api.ServiceConfiguration
{
    public class AuthorizationConfig : IServiceCoreConfiguration
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            var sp = services.BuildServiceProvider();
            var userAccessService = sp.GetService<IUserAccessService>();
            var policies = userAccessService.GetPolicies();
            // Configure your policies

            foreach (var item in policies)
            {
                if (item.RoleCodes.Any())
                {
                    services.AddAuthorization(options =>
                       options.AddPolicy(item.PolicyCode,
                       policy =>
                       {
                           foreach (var role in item.RoleCodes)
                           {
                               policy.RequireClaim("CLAIM_" + role);
                           }
                       }));
                }
            }
        }
    }
}