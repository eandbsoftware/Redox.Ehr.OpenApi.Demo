using Flurl.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Redox.Ehr.Contracts.Models.Redox.Patientsearch.Response;
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
    public class EhrPatientSearchController : BaseController<EhrPatientSearchController>
    {
        private readonly IPatientSearchService _patientSearchService;

        public EhrPatientSearchController(IPatientSearchService patientSearchService)
        {
            _patientSearchService = patientSearchService;
        }

        /// <summary>
        /// Search Patient by NIST ID value
        /// </summary>
        /// <param name="id">NIST ID</param>
        /// <returns>Response</returns>
        [HttpGet("searchById", Name = "SearchPatientByIdAsync")]
        [ProducesResponseType(200, Type = typeof(Response))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> SearchPatientByIdAsync([FromQuery] string id)
        {
            Logger.LogDebug(nameof(SearchPatientByIdAsync));
            try
            {
                var response = await _patientSearchService.SearchPatientAsync(id);
                return Ok(response);
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseStringAsync();
                var jsonError = JObject.Parse(error);
                return NotFound(jsonError);
            }
        }

        /// <summary>
        /// Search Patient by Firstname and/or LastName
        /// </summary>
        /// <param name="firstName">Patient's first name</param>
        /// <param name="lastName">Patient's last name</param>
        /// <returns>Response</returns>
        [HttpGet("search", Name = "SearchPatientAsync")]
        [ProducesResponseType(200, Type = typeof(Response))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> SearchPatientAsync([FromQuery] string firstName, [FromQuery] string lastName)
        {
            Logger.LogDebug(nameof(SearchPatientAsync));
            try
            {
                var response = await _patientSearchService.SearchPatientAsync(firstName, lastName);
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