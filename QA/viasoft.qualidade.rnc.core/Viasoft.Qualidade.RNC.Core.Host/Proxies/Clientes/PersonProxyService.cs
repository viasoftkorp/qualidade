using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.IoC.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.Clientes;

public class PersonProxyService : BaseProxyService<PersonOutput>, IPersonProxyService, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private const string ServiceName = "Viasoft.ERP.Person";
    private const string BasePath = "/erp/person/person";

    public PersonProxyService(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }

    public override Task<ListResultDto<PersonOutput>> GetAll(PagedFilteredAndSortedRequestInput filter)
    {
        throw new NotImplementedException();
    }

    public override async Task<PersonOutput> GetById(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/getbyidv3/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var cliente = await callBuilder.ResponseCallAsync<PersonOutput>();
        return cliente;
    }

    protected override JsonNetFilterRule GetGetAllAdvancedFilter(object value)
    {
        throw new NotImplementedException();
    }
}