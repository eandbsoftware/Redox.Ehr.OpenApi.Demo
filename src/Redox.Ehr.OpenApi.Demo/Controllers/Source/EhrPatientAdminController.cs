using Flurl.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Redox.Ehr.Contracts.Converters;
using Redox.Ehr.Contracts.Models.Dto;
using Redox.Ehr.Contracts.Models.Redox.Patientadmin.Newpatient;
using Redox.Ehr.OpenApi.Controllers.Base;
using Redox.Ehr.OpenApi.Services;
using System.Threading.Tasks;

namespace Redox.Ehr.OpenApi.Controllers.Source
{
    /// <summary>
    /// Redox SOURCE OpenAPI controller
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("EnableCORS")]
    [Route("api/[controller]")]
    public class EhrPatientAdminController : BaseController<EhrPatientAdminController>
    {
        private readonly IPatientAdminService _patientAdminService;
        private readonly IConverter<PatientDto, Newpatient> _patientDtoToNewpatientCoverter;

        public EhrPatientAdminController(IPatientAdminService patientAdminService, IConverter<PatientDto, Newpatient> patientDtoToNewpatientCoverter)
        {
            _patientAdminService = patientAdminService;
            _patientDtoToNewpatientCoverter = patientDtoToNewpatientCoverter;
        }

        [HttpPost("newPatient", Name = "NewPatientAsync")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> NewPatientAsync([FromBody] PatientDto patient)
        {
            Logger.LogDebug(nameof(NewPatientAsync));
            try
            {
                var newpatient = _patientDtoToNewpatientCoverter.Convert(patient);
                var response = await _patientAdminService.NewPatientAsync(newpatient);
                return Ok(response);
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseStringAsync();
                var jsonError = JObject.Parse(error);
                return NotFound(jsonError);
            }
        }
    }
}