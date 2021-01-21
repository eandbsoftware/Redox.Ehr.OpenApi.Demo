using Redox.Ehr.Contracts.Models.Redox.Patientsearch.Response;
using System.Threading.Tasks;

namespace Redox.Ehr.OpenApi.Services
{
    public interface IPatientSearchService
    {
        Task<Response> SearchPatientAsync(string id);

        Task<Response> SearchPatientAsync(string firstName, string lastName);
    }
}