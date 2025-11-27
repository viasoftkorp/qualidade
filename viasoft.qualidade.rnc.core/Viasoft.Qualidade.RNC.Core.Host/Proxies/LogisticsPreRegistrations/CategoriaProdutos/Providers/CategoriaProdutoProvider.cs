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
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsPreRegistrations.CategoriaProdutos.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsPreRegistrations.CategoriaProdutos.Providers;

public class CategoriaProdutoProvider : ICategoriaProdutoProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private const int MaxResultCount = 50;
    private const string ServiceName = "Viasoft.Logistics.PreRegistration";
    private const string BasePath = "/logistics/preregistration/categorias-produto";

    public CategoriaProdutoProvider(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }

    public async Task<PagedResultDto<CategoriaProdutoOutput>> GetList(PagedFilteredAndSortedRequestInput input)
    {
        var queryParameters = input.ToHttpGetQueryParameter();
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}?{queryParameters}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();
        var output = await callBuilder.ResponseCallAsync<PagedResultDto<CategoriaProdutoOutput>>();

        return output;
    }
    public async Task<List<CategoriaProdutoOutput>> GetAllCategoriasPaginando(List<Guid> idsCategorias)
    {
        if (!idsCategorias.Any())
        {
            return new List<CategoriaProdutoOutput>();
        }

        var skipCount = 0;
        long totalCount;
        var categorias = new List<CategoriaProdutoOutput>();
        do
        {
            var advancedFilter = new JsonNetFilterRule
            {
                Condition = "AND",
                Rules = new List<JsonNetFilterRule>
                {
                    new JsonNetFilterRule()
                    {
                        Field = "Id",
                        Operator = "in",
                        Type = "string",
                        Value = idsCategorias
                    }
                }
            };

            var filter = new PagedFilteredAndSortedRequestInput
            {
                AdvancedFilter = JsonConvert.SerializeObject(advancedFilter),
                MaxResultCount = 50,
                SkipCount = skipCount
            };
            var pagedProdutos = await GetList(filter);
            categorias.AddRange(pagedProdutos.Items);
            totalCount = pagedProdutos.TotalCount;
            skipCount += MaxResultCount;
        } while (categorias.Count < totalCount);

        return categorias;
    }
}