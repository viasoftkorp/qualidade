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
using Viasoft.Qualidade.RNC.Gateway.Domain.Utils;
using Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LogisticsProducts.Providers;

public class ProdutoProvider : IProdutoProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;

    private const string ServiceName = "Viasoft.Logistics.Products";
    private const string BasePath = "/logistics/products/produtos";
    
    public ProdutoProvider(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }

    public async Task<PagedResultDto<ProdutoOutput>> GetProdutosList(PagedFilteredAndSortedRequestInput input)
    {
        
        var queryParameters = input.ToHttpGetQueryParameter();
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}?{queryParameters}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();
        var output = await callBuilder.ResponseCallAsync<PagedResultDto<ProdutoOutput>>();
        
        return output;
    }
    
    public async Task<PagelessResultDto<ProdutoOutput>> GetPagelessProdutosList(PagedFilteredAndSortedRequestInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"/Logistics/Products/Product/GetProductsByCode3")
            .WithTimeout(ApiServiceCallTimeout.UltraLong)
            .WithHttpMethod(HttpMethod.Post)
            .WithBody(input)
            .Build();
        var result = await callBuilder.ResponseCallAsync<PagelessResultDto<ProductOutput>>();

        var outputItems = result.Items.Select(e => new ProdutoOutput(e)).ToList();

        var output = new PagelessResultDto<ProdutoOutput>(outputItems);
        
        return output;
    }

    public async Task<ProdutoOutput> GetProduto(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var output = await callBuilder.ResponseCallAsync<ProdutoOutput>();
        return output;
    }

    public async Task<ProdutoOutput> GetByCode(string code)
    {
        var advancedFilter = new JsonNetFilterRule
        {
            Condition = "and",
            Rules = new List<JsonNetFilterRule>
            {
                new()
                {
                    Field = "Code",
                    Operator = "equal",
                    Type = "string",
                    Value = code
                }
            }
        };
        var input = new PagedFilteredAndSortedRequestInput
        {
            AdvancedFilter = JsonConvert.SerializeObject(advancedFilter),
            MaxResultCount = 1,
        };
        
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}?{input.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var response = await callBuilder.ResponseCallAsync<PagedResultDto<ProdutoOutput>>();
        return response.Items.First();
    }
}