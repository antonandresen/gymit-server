using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using gymit.Contracts.V1;
using gymit.Services;

namespace gymit.Controllers.V1
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route(ApiRoutes.Base)]
    public class TagsController : ControllerBase
    {
        private readonly ITestService _testService;

        public TagsController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _testService.GetAllTagsAsync());
        }
    }
}
