using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Otter.Core.Account;
using Otter.Core.Constant;
using Otter.Services.Account;
using Otter.Services.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otter.Api.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        /// <summary>
        /// Get List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var data = await _companyService.GetList();
            return Ok(new
            {
                success = true,
                data
            });
        }
    }
}