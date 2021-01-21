using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Redox.DataModel.Builder.Configuration;
using Redox.Ehr.Contracts.Models.Redox.Medications.New;
using Redox.Ehr.Contracts.Models.Redox.Medications.Cancel;
using Redox.Ehr.Contracts.Models.Redox.Medications.Administration;
using Redox.Ehr.Contracts.Models.Redox.Medications.Update;
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
    public class MedicationsController : BaseController<MedicationsController>
    {
        private readonly RedoxApiConfig _config;

        public MedicationsController(IOptions<RedoxApiConfig> config)
        {
            _config = config.Value;
        }

        [HttpPost("Administration")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Administration([FromBody] Administration medications)
        {
            Logger.LogDebug(nameof(Administration));
            if (!IsAuthenticated())
            {
                return BadRequest();
            }

            // TODO: Patient received a given medication. Not implemented, yet.

            return Ok();
        }

        [HttpPost("Cancel")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Cancel([FromBody] Cancel medications)
        {
            Logger.LogDebug(nameof(Cancel));
            if (!IsAuthenticated())
            {
                return BadRequest();
            }

            // TODO: A previous medication request has been cancelled by the ordering system. Not implemented, yet.

            return Ok();
        }

        [HttpPost("New")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult New([FromBody] New medications)
        {
            Logger.LogDebug(nameof(New));
            if (!IsAuthenticated())
            {
                return BadRequest();
            }

            // TODO: A new request for a medication to be dispensed for the given patient. Not implemented, yet.

            return Ok();
        }

        [HttpPost("Update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Update([FromBody] Update medications)
        {
            Logger.LogDebug(nameof(Update));
            if (!IsAuthenticated())
            {
                return BadRequest();
            }

            // TODO: Part of a medication order has been changed by the ordering system. Not implemented, yet.

            return Ok();
        }
    }
}
