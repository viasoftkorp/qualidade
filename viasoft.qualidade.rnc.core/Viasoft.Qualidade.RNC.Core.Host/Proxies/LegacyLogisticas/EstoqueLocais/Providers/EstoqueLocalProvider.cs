using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.EstoqueLocais.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.EstoqueLocais.Providers;

public class EstoqueLocalProvider: IEstoqueLocalProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private readonly ICurrentCompany _currentCompany;
    private const string ServiceName = "Viasoft.Legacy.Logistica";
    private const string BasePath = "legacy/logistica/estoques-locais";
    
    public EstoqueLocalProvider(IApiClientCallBuilder apiClientCallBuilder,
        ICurrentCompany currentCompany)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
        _currentCompany = currentCompany;
    }
    public async Task<PagedResultDto<EstoqueLocal>> GetList(GetListEstoqueLocalInput input)
    {
        input.IdEmpresa = _currentCompany.Id;
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}?{input.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var estoquesLocais = await callBuilder.ResponseCallAsync<PagedResultDto<EstoqueLocal>>();
        return estoquesLocais;
    }

    public async Task<EstoqueLocal> GetById(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var estoqueLocal = await callBuilder.ResponseCallAsync<EstoqueLocal>();
        return estoqueLocal;
    }
    public async Task<EstoqueLocal> GetByLegacyId(int legacyId)
    {
        var advancedFilter = new JsonNetFilterRule
        {
            Condition = "AND",
            Rules = new List<JsonNetFilterRule>
            {
                new JsonNetFilterRule()
                {
                    Field = "LegacyId",
                    Operator = "equal",
                    Type = "integer",
                    Value = legacyId
                }
            }
        };

        var input = new GetListEstoqueLocalInput
        {
            AdvancedFilter = JsonConvert.SerializeObject(advancedFilter),
            MaxResultCount = 1,
            SkipCount = 0
        };
        var result = await GetList(input);
        return result.Items.First();
    }
}