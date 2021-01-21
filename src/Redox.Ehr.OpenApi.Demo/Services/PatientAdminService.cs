using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Redox.DataModel.Builder.Configuration;
using Redox.Ehr.Contracts.Models.Redox.Patientadmin.Newpatient;
using System.Threading.Tasks;
using System;
using System.Collections.ObjectModel;
using Meta = Redox.Ehr.Contracts.Models.Redox.Patientadmin.Newpatient.Meta;

namespace Redox.Ehr.OpenApi.Services
{
    // https://developer.redoxengine.com/questions-and-answers/understanding-redox-patientadmin-data-model/
    public class PatientAdminService : IPatientAdminService
    {
        private readonly ILogger<PatientAdminService> _logger;
        private readonly IAuthService _authService;
        private readonly IRedoxClient _client;
        private readonly RedoxApiConfig _config;

        public PatientAdminService(ILogger<PatientAdminService> logger,
            IOptions<RedoxApiConfig> config,
            IAuthService authService, IRedoxClient client)
        {
            _logger = logger;
            _authService = authService;
            _client = client;
            _config = config.Value;
        }

        public async Task<object> NewPatientAsync(Newpatient patient)
        {
            _logger.LogDebug(nameof(NewPatientAsync));

            patient.Meta = new Meta
            {
                DataModel = "PatientAdmin",
                EventType = "NewPatient",
                EventDateTime = DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture),
                Destinations = new Collection<Destinations>
                    {
                        new Destinations()
                        {
                            ID = _config.PatientAdmin_Destination_ID,
                            Name = _config.PatientAdmin_Destination_Name
                        }
                    }
            };

            var result = await _client.PostAsync<object>(patient);
            return result;
        }
    }
}
