using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Redox.DataModel.Builder.Configuration;
using Redox.Ehr.Contracts.Models.Redox.Organization.New;
using Redox.Ehr.Contracts.Models.Redox.Organization.Update;
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
    public class OrganizationController : BaseController<OrganizationController>
    {
        private readonly RedoxApiConfig _config;

        public OrganizationController(IOptions<RedoxApiConfig> config)
        {
            _config = config.Value;
        }

        [HttpPost("New")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult New([FromBody] New organization)
        {
            Logger.LogDebug(nameof(New));
            if (!IsAuthenticated())
            {
                return BadRequest();
            }

            // TODO: New information about an organization as a whole from an external organization directory, such as Carequality. Not implemented, yet.

            return Ok();
        }

        [HttpPost("Update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Update([FromBody] Update organization)
        {
            Logger.LogDebug(nameof(Update));
            if (!IsAuthenticated())
            {
                return BadRequest();
            }

            // TODO: Updated information about an organization as a whole from an external organization directory, such as Carequality. Not implemented, yet.

            return Ok();
        }
    }
}
