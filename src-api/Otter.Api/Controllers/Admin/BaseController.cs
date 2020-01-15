using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Otter.Core.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otter.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]/[action]")]
    [Authorize(Policy = PolicyCodes.ADMIN)]
    public class BaseController : ControllerBase
    {
    }
}
