using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using gymit.Models;
using gymit.Contracts.V1;
using gymit.Contracts.V1.Requests;
using gymit.Contracts.V1.Responses;
using gymit.Services;
using gymit.Extensions;


namespace gymit.Controllers.V1
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route(ApiRoutes.Base)]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;
        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _testService.GetTestsAsync());
        }

        [HttpGet("{testID:Guid}")]
        public async Task<IActionResult> GetOne([FromRoute] Guid testID)
        {
            var test = await _testService.GetTestByIDAsync(testID);

            if(test == null)
            {
                return NotFound();
            }

            return Ok(test);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTestRequest testRequest)
        {
            var test = new Test { 
                Number = testRequest.Number, 
                Text = testRequest.Text,
                Date = DateTime.Now,
                UserId = HttpContext.GetUserId()
            };

            await _testService.CreateTestAsync(test);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.BareBase + "/test/" + test.ID.ToString();

            var response = new TestResponse { ID = test.ID };
            return Created(locationUri, response);
        }

        [HttpPut("{testID:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid testID, [FromBody] UpdateTestRequest testRequest)
        {
            var userOwnsTest = await _testService.UserOwnsTestAsync(testID, HttpContext.GetUserId());
            if (!userOwnsTest)
            {
                return BadRequest(new { error = "You do not own this test" });
            }

            var test = await _testService.GetTestByIDAsync(testID);
            test.Text = testRequest.Text;

            var updated = await _testService.UpdateTestAsync(test);
            if (updated) return Ok(test);

            return NotFound();
        }

        [HttpDelete("{testID:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid testID)
        {
            var userOwnsTest = await _testService.UserOwnsTestAsync(testID, HttpContext.GetUserId());
            if (!userOwnsTest)
            {
                return BadRequest(new { error = "You do not own this test" });
            }

            var deleted = await _testService.DeleteTestAsync(testID);

            if (deleted) return NoContent();

            return NotFound();
        }
    }
}
