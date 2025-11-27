using Asp.Versioning;
using Viasoft.Core.AmbientData.Abstractions;
using Viasoft.Core.FileProvider.Proxy;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Files;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
public class FileProxyController : FileProviderProxyController
{
    public FileProxyController(IFileProviderProxyService fileProviderProxyService,
        IAmbientDataCallOptionsResolver ambientDataCallOptionsResolver) : base(fileProviderProxyService,
        ambientDataCallOptionsResolver)
    {
    }
}