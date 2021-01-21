using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Redox.DataModel.Builder.Configuration;
using Redox.Ehr.Contracts.Models.Redox.Patientsearch.Query;
using Redox.Ehr.Contracts.Models.Redox.Patientsearch.Response;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using Demographics = Redox.Ehr.Contracts.Models.Redox.Patientsearch.Query.Demographics;
using Destinations = Redox.Ehr.Contracts.Models.Redox.Patientsearch.Query.Destinations;
using Identifiers = Redox.Ehr.Contracts.Models.Redox.Patientsearch.Query.Identifiers;
using Meta = Redox.Ehr.Contracts.Models.Redox.Patientsearch.Query.Meta;
using Patient = Redox.Ehr.Contracts.Models.Redox.Patientsearch.Query.Patient;

namespace Redox.Ehr.OpenApi.Services
{
    public class PatientSearchService : IPatientSearchService
    {
        private readonly ILogger<PatientSearchService> _logger;
        private readonly IRedoxClient _client;
        private readonly RedoxApiConfig _config;

        public PatientSearchService(ILogger<PatientSearchService> logger,
            IOptions<RedoxApiConfig> config, IRedoxClient client)
        {
            _logger = logger;
            _client = client;
            _config = config.Value;
        }

        public async Task<Response> SearchPatientAsync(string id)
        {
            _logger.LogDebug(nameof(SearchPatientAsync));

            // Build search query
            var query = PatientSearchQuery();
            query.Patient = new Patient()
            {
                Identifiers = new Collection<Identifiers>()
                    {
                        new Identifiers()
                        {
                            ID = id,
                            IDType = "NIST"
                        }
                    }
            };

            // Make a call
            var result = await _client.QueryAsync<Response>(query);

            return result;
        }

        public async Task<Response> SearchPatientAsync(string firstName, string lastName)
        {
            _logger.LogDebug(nameof(SearchPatientAsync));

            // Build search query
            var query = PatientSearchQuery();
            query.Patient = new Patient()
            {
                Demographics = new Demographics()
                {
                    FirstName = firstName,
                    LastName = lastName
                }
            };

            // Make a call
            var result = await _client.QueryAsync<Response>(query);

            return result;
        }

        private Query PatientSearchQuery()
        {
            var query = new Query
            {
                Meta = new Meta()
                {
                    DataModel = "PatientSearch",
                    EventType = "Query",
                    EventDateTime = DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture),
                    Test = true,
                    Destinations = new Collection<Destinations>
                    {
                        new Destinations()
                        {
                            ID = _config.PatientSearch_Destination_ID,
                            Name = _config.PatientSearch_Destination_Name
                        }
                    }
                }
            };
            return query;
        }
    }
}
