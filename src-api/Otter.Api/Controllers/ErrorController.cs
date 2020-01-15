using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Otter.Api.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this._logger = logger;
        }

        [HttpGet("/test")]
        public string test()
        {
            return "this is a test";
        }

        [HttpGet("/error-dev")]
        [HttpPost("/error-dev")]
        [HttpDelete("/error-dev")]
        [HttpPut("/error-dev")]
        public async Task ErrorDevelopment([FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException(
                    "This shouldn't be invoked in non-development environments.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (context != null && context.Error is SecurityTokenExpiredException)
            {
                HttpContext.Response.StatusCode = 401;
                HttpContext.Response.ContentType = "application/json";

                await HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    success = false,
                    State = "Unauthorized",
                    Msg = "token expired"
                }));
                return;
            }

            HttpContext.Response.StatusCode = 500;
            HttpContext.Response.ContentType = "application/json";

            await HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                success = false,
                message = context.Error.Message,
                detail = context.Error.StackTrace,
            }));
        }
    }
}