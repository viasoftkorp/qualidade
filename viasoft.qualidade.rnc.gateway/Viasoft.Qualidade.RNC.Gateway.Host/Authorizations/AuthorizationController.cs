using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller.Authorization;
using Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations.Policies;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Authorizations;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
public class AuthorizationController : BaseAuthorizationController
{
    [HttpGet("/qualidade/rnc/gateway/authorization/permissions")]
    public override Task<List<string>> GetAuthorizations()
    {
        var policies = ToList(typeof(Policies));
        var retrabalhoPolicies = ToList(typeof(RetrabalhoPolicies));

        var allPolicies = policies.Concat(retrabalhoPolicies).ToList();
        
        return Task.FromResult(allPolicies);
    }
    private List<string> ToList(Type type)
    {
        List<string> result = new List<string>();

        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == typeof(string))
            {
                result.Add((string)field.GetValue(null));
            }
        }

        return result;
    }
}