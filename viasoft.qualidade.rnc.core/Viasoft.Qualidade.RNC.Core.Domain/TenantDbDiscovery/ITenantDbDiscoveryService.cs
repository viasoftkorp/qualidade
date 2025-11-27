using System.Threading.Tasks;

namespace Viasoft.Qualidade.RNC.Core.Domain.TenantDbDiscovery
{
    public interface ITenantDbDiscoveryService
    {
        Task<string> DbName();
        Task<string> ServerIp();
    }
}