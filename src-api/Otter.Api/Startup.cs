using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Otter.Core;
using Otter.Api.ServiceConfiguration;
using Otter.Services;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Linq;
using FluentValidation.AspNetCore;

namespace Otter.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            WebHostEnvironment = env;
        }
        private IWebHostEnvironment WebHostEnvironment { get; set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(
                //options => options.Filters.Add(new HttpResponseExceptionFilter())
                )
                .AddNewtonsoftJson()
                ;

            services.AddCors();

            services.AddServices(Configuration);
            services.AddServices(Configuration, WebHostEnvironment);

            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Core.Account.IValidate>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error-dev");

                #region Check Later

                //app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler(appBuilder =>
                //{
                //    appBuilder.Use(async (context, next) =>
                //    {
                //        var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

                //        //when authorization has failed, should retrun a json message to client
                //        if (error != null && error.Error is SecurityTokenExpiredException)
                //        {
                //            context.Response.StatusCode = 401;
                //            context.Response.ContentType = "application/json";

                //            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                //            {
                //                State = "Unauthorized",
                //                Msg = "token expired"
                //            }));
                //        }
                //        //when orther error, retrun a error message json to client
                //        else if (error != null && error.Error != null)
                //        {
                //            context.Response.StatusCode = 500;
                //            context.Response.ContentType = "application/json";
                //            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                //            {
                //                State = "Internal Server Error",
                //                Msg = error.Error.Message
                //            }));
                //        }
                //        //when no error, do next.
                //        else await next();
                //    });
                //});

                #endregion Check Later
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Otter Core API");
                c.RoutePrefix = string.Empty;
                c.DocExpansion(DocExpansion.None);
            });

            app.UseHttpsRedirection();

            #region Added by JWT Example

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            #endregion Added by JWT Example

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });
        }
    }
}