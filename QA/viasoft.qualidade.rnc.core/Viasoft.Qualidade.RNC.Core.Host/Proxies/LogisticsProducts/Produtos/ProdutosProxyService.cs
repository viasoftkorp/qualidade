using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.Utils;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsProducts.Produtos;

public class ProdutosProxyService : BaseProxyService<ProdutoOutput>, IProdutosProxyService, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private const string ServiceName = "Viasoft.Logistics.Products";
    private const string BasePath = "/logistics/products/produtos";

    public ProdutosProxyService(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }
    public override async Task<ProdutoOutput> GetById(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var output = await callBuilder.ResponseCallAsync<ProdutoOutput>();
        return output;
    }
    public override async Task<ListResultDto<ProdutoOutput>> GetAll(PagedFilteredAndSortedRequestInput filter)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"/Logistics/Products/Product/GetProductsByCode3")
            .WithBody(filter)
            .WithTimeout(ApiServiceCallTimeout.UltraLong)
            .WithHttpMethod(HttpMethod.Post)
            .Build();
        var result = await callBuilder.ResponseCallAsync<PagelessResultDto<ProductOutput>>();

        var itens = result.Items.ConvertAll(e => new ProdutoOutput
        {
            Id = e.Id,
            Codigo = e.Code,
            Descricao = e.Description,
            IdUnidade = e.MeasureId,
            IdCategoria = e.CategoryId
        });
        
        return new PagelessResultDto<ProdutoOutput>
        {
            Items = itens
        };
        
    }

    protected override JsonNetFilterRule GetGetAllAdvancedFilter(object value)
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
                    Value = value
                }
            }
        };
        return advancedFilter;
    }
}

