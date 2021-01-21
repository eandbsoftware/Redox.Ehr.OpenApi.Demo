using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Redox.DataModel.Builder.Configuration;
using Redox.Ehr.Contracts.Models;
using Redox.Ehr.OpenApi.Controllers.Base;

namespace Redox.Ehr.OpenApi.Controllers.Destination
{
    /// <summary>
    /// Redox DESTINATION OpenAPI controller
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Produces("application/json")]
    [EnableCors("EnableCORS")]
    [Route("api/[controller]")]
    public class RedoxController : BaseController<RedoxController>
    {
        private readonly RedoxApiConfig _config;

        public RedoxController(IOptions<RedoxApiConfig> config)
        {
            _config = config.Value;
        }

        [HttpPost("VerifyDestination")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult VerifyDestination([FromBody] Verification verification)
        {
            Logger.LogDebug(nameof(VerifyDestination));
            if (!verification.Verification_token.Equals(_config.RedoxDestinationVerificationToken))
            {
                return BadRequest();
            }
            // Reply to Redox
            return Ok(verification.Challenge);
        }
    }
}
