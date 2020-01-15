using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Otter.Core;
using Otter.Core.Account;
using Otter.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Otter.Api.ServiceConfiguration
{
    public class AuthenticationConfig : IServiceCoreConfiguration
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            // configure strongly typed settings objects
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            services.AddHttpClient();
            services.AddHttpContextAccessor();

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtOptions =>
            {
                jwtOptions.SaveToken = true;
                jwtOptions.RequireHttpsMetadata = false;

                var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);
                jwtOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "OTTER",
                    ValidIssuer = "OTTER",
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };
                jwtOptions.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json; charset=utf-8";
                        
                        var message = webHostEnvironment.IsDevelopment() ? context.Exception.ToString() : "An error occurred processing your authentication.";
                        var result = JsonConvert.SerializeObject(new { message });
                        return context.Response.WriteAsync(result);
                    }
                    //OnChallenge = context =>
                    //{
                    //    context.HandleResponse();
                    //    return Task.CompletedTask;
                    //}
                };
            })
            ;

            // configure DI for application services

            services.AddScoped<ICurrentUser, CurrentUser>();
        }
    }
}