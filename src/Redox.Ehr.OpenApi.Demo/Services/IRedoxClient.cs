using System.Threading.Tasks;

namespace Redox.Ehr.OpenApi.Services
{
    public interface IRedoxClient
    {
        Task<T> PostAsync<T>(object data) where T : new();

        Task<T> QueryAsync<T>(object data) where T : new();
    }
}