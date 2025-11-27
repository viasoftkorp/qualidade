using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoqueLocais.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyParametros.Providers;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoqueLocais.Providers;

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
    public async Task<PagedResultDto<EstoqueLocalOutput>> GetList(GetListEstoqueLocalInput input)
    {
        input.IdEmpresa = _currentCompany.Id;
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}?{input.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var estoquesLocais = await callBuilder.ResponseCallAsync<PagedResultDto<EstoqueLocalOutput>>();
        return estoquesLocais;
    }

    public async Task<EstoqueLocalOutput> GetById(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var estoqueLocal = await callBuilder.ResponseCallAsync<EstoqueLocalOutput>();
        return estoqueLocal;
    }
}