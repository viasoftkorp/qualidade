using Asp.Versioning;
using Viasoft.Core.AmbientData.Abstractions;
using Viasoft.Core.AmbientData.Attributes;
using Viasoft.Core.Authorization.Proxy;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Authentication
{
    [ApiVersion(2024.1)]
    [ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
    [ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
    [ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    [AmbientDataNotRequired]
    public class AuthorizationProxyController : Viasoft.Core.Authorization.Proxy.AuthorizationProxyController
    {
        public AuthorizationProxyController(IAuthorizationProxyService authorizationProxyService,
            IAmbientDataCallOptionsResolver ambientDataCallOptionsResolver) : base(authorizationProxyService,
            ambientDataCallOptionsResolver)
        {
        }
    }
}