using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Vendas.ErpPerson.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Vendas.ErpPerson.Providers;

public class PersonProvider : IPersonProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private const string ServiceName = "Viasoft.ERP.Person";
    private const string BasePath = "/erp/person/pessoas";

    public PersonProvider(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }

    public async Task<PersonOutput> GetById(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var cliente = await callBuilder.ResponseCallAsync<PersonOutput>();
        return cliente;
    }
    public async Task<PagedResultDto<PersonOutput>> GetAll(GetAllPersonInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}?{input.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var cliente = await callBuilder.ResponseCallAsync<PagedResultDto<PersonOutput>>();
        return cliente;
    }
}