using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication_Startup_Logging.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MyCrudController : ControllerBase
    {
        private ILogger<MyCrudController> _logger;

        public MyCrudController(ILogger<MyCrudController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]Job obj)
        {
            return Ok();
        }

    }
}
