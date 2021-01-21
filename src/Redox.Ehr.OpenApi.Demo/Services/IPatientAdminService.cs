using Redox.Ehr.Contracts.Models.Redox.Patientadmin.Newpatient;
using System.Threading.Tasks;

namespace Redox.Ehr.OpenApi.Services
{
    public interface IPatientAdminService
    {
        Task<object> NewPatientAsync(Newpatient patient);
    }
}