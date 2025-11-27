using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.AmbientData;
using Viasoft.Core.AmbientData.Extensions;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsProducts.ProdutosEmpresas.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsProducts.ProdutosEmpresas;

public class ProdutoEmpresaProxyService : BaseProxyService<ProdutoEmpresaOutput>, IProdutoEmpresaProxyService, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private readonly IAmbientData _ambientData;
    private const string ServiceName = "Viasoft.Logistics.Products";
    private const string BasePath = "logistics/products/company-products";

    public ProdutoEmpresaProxyService(IApiClientCallBuilder apiClientCallBuilder, IAmbientData ambientData)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
        _ambientData = ambientData;
    }

    public override async Task<ProdutoEmpresaOutput> GetById(Guid id)
    {
        var input = new GetProdutoEmpresaListInput
        {
            ProductsIds = new List<Guid>{ id },
            CompanyId = _ambientData.GetCompanyId(),
            MaxResultCount = 1
        };

        var filter = input.ToHttpGetQueryParameter();
        var productsIds = input.ProductsIds.ToHttpGetQueryParameter(nameof(GetProdutoEmpresaListInput.ProductsIds));
        
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}?{filter}&{productsIds}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var response = await callBuilder.ResponseCallAsync<PagedResultDto<ProdutoEmpresaOutput>>();

        var output = response.Items.First();
        
        return output;
    }

    public async Task<PagedResultDto<ProdutoEmpresaOutput>> GetAll(GetProdutoEmpresaListInput input)
    {
        var queryParameters = input.ToHttpGetQueryParameter();

        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}?{queryParameters}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var response = await callBuilder.ResponseCallAsync<PagedResultDto<ProdutoEmpresaOutput>>();

        return response;
    }

    public override Task<ListResultDto<ProdutoEmpresaOutput>> GetAll(PagedFilteredAndSortedRequestInput filter)
    {
        throw new NotImplementedException();
    }

    protected override JsonNetFilterRule GetGetAllAdvancedFilter(object value)
    {
        throw new NotImplementedException();
    }
}

