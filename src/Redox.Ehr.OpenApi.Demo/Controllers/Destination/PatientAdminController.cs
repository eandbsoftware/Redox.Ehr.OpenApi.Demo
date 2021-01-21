using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Redox.DataModel.Builder.Configuration;
using Redox.Ehr.Contracts.Models.Redox.Patientadmin.Arrival;
using Redox.Ehr.Contracts.Models.Redox.Patientadmin.Cancel;
using Redox.Ehr.Contracts.Models.Redox.Patientadmin.Newpatient;
using Redox.Ehr.Contracts.Models.Redox.Patientadmin.Patientupdate;
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
    public class PatientAdminController : BaseController<PatientAdminController>
    {
        private readonly RedoxApiConfig _config;

        public PatientAdminController(IOptions<RedoxApiConfig> config)
        {
            _config = config.Value;
        }

        [HttpPost("Arrival")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Arrival([FromBody] Arrival patient)
        {
            Logger.LogDebug(nameof(Arrival));
            if (!IsAuthenticated())
            {
                return BadRequest();
            }

            // TODO: A patient shows up for their visit/a patient is admitted to the hospital. Not implemented, yet.

            return Ok();
        }

        [HttpPost("Cancel")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Cancel([FromBody] Cancel patient)
        {
            Logger.LogDebug(nameof(Cancel));
            if (!IsAuthenticated())
            {
                return BadRequest();
            }

            // TODO: A PatientAdmin event is canceled. Not implemented, yet.

            return Ok();
        }

        [HttpPost("NewPatient")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult NewPatient([FromBody] Newpatient patient)
        {
            Logger.LogDebug(nameof(NewPatient));
            if (!IsAuthenticated())
            {
                return BadRequest();
            }

            // TODO: A new patient has been created in the source EHR system. Not implemented, yet.

            return Ok();
        }

        [HttpPost("PatientUpdate")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult PatientUpdate([FromBody] Patientupdate patient)
        {
            Logger.LogDebug(nameof(PatientUpdate));
            if (!IsAuthenticated())
            {
                return BadRequest();
            }

            // TODO: A patient or registration information changed while the patient is not currently admitted. Not implemented, yet.

            return Ok();
        }
    }
}
