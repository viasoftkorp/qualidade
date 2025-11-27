using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.IoC.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.Usuarios;

public class UsuarioProxyService : BaseProxyService<UsuarioOutput>, IUsuarioProxyService, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private const string ServiceName = "Viasoft.Authentication";
    private const string BasePath = "oauth/Users";

    public UsuarioProxyService(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }

    public override async Task<ListResultDto<UsuarioOutput>> GetAll(PagedFilteredAndSortedRequestInput filter)
    {
        var queryParameters = filter.ToHttpGetQueryParameter();

        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/GetAll?{queryParameters}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var response = await callBuilder.ResponseCallAsync<PagedResultDto<UsuarioOutput>>();

        var output = new ListResultDto<UsuarioOutput>(response.Items);
        
        return output;
    }

    public override async Task<UsuarioOutput> GetById(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/GetById/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var usuario = await callBuilder.ResponseCallAsync<UsuarioOutput>();
        return usuario;
    }

    protected override JsonNetFilterRule GetGetAllAdvancedFilter(object value)
    {
        var advancedFilter = new JsonNetFilterRule
        {
            Condition = "and",
            Rules = new List<JsonNetFilterRule>
            {
                new()
                {
                    Field = "Id",
                    Operator = "in",
                    Type = "string",
                    Value = value
                }
            }
        };

        return advancedFilter;
    }
}