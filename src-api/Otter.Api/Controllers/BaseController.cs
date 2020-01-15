using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Otter.Core.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otter.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize()]
    public class BaseController : ControllerBase
    {
    }
}
