using System.Threading.Tasks;
using adform.Services;
using adform.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace adform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IAuthorizationService _authorization;
        private readonly IReportsService _reportsService;

        public ReportsController(IAuthorizationService authorization,
            IReportsService reportsService)
        {
            _authorization = authorization;
            _reportsService = reportsService;
        }


        [HttpGet("weeks")]
        [Produces(typeof(ReportResult[]))]
        public async Task<IActionResult> GetBid()
        {
            var token = await _authorization.GetToken();
            if (token == null)
            {
                return StatusCode(500, "Unable to authorize");
            }
            var result = await _reportsService.GetWeeks(token);

            return Ok(result);
        }


        [HttpGet("anomalies")]
        public async Task<IActionResult> GetAnomalies()
        {
            var token = await _authorization.GetToken();
            if (token == null)
            {
                return StatusCode(500, "Unable to authorize");
            }
            var result = await _reportsService.GetAnomalies(token);

            return Ok(result);
        }


    }
}