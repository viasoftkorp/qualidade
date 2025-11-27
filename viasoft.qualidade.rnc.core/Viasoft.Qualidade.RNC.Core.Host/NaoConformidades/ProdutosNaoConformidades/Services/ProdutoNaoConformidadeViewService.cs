using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Data.Extensions.Filtering.AdvancedFilter;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.UnidadeMedidaProdutos;
using Viasoft.Qualidade.RNC.Core.Domain.ProdutoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Services;

public class ProdutoNaoConformidadeViewService : IProdutoNaoConformidadeViewService, ITransientDependency
{
    private readonly IRepository<ProdutoNaoConformidade> _produtoNaoConformidades;
    private readonly IRepository<Produto> _produtos;
    private readonly IRepository<UnidadeMedidaProduto> _unidadeMedidaProdutos;
    private readonly ICurrentCompany _currentCompany;

    public ProdutoNaoConformidadeViewService(
        IRepository<ProdutoNaoConformidade> produtoNaoConformidades,
        IRepository<Produto> produtos, IRepository<UnidadeMedidaProduto> unidadeMedidaProdutos,
        ICurrentCompany currentCompany)
    {
        _produtoNaoConformidades = produtoNaoConformidades;
        _produtos = produtos;
        _unidadeMedidaProdutos = unidadeMedidaProdutos;
        _currentCompany = currentCompany;
    }

    public async Task<PagedResultDto<ProdutoNaoConformidadeViewOutput>> GetListView(Guid idNaoConformidade,
        PagedFilteredAndSortedRequestInput input)
    {
        var query = (from produtoSolucao in _produtoNaoConformidades
                where produtoSolucao.CompanyId == _currentCompany.Id
                join produto in _produtos
                        on produtoSolucao.IdProduto equals produto.Id into produtoJoinedTable
                from produto in produtoJoinedTable.DefaultIfEmpty()
                join unidadeMedida in _unidadeMedidaProdutos
                    on produto.IdUnidadeMedida equals unidadeMedida.Id into unidadeJoinedTable
                from unidadeMedida in unidadeJoinedTable.DefaultIfEmpty()
                select new ProdutoNaoConformidadeViewOutput
                {
                    Id = produtoSolucao.Id,
                    IdProduto = produtoSolucao.IdProduto,
                    Codigo = produto.Codigo,
                    Descricao = produto.Descricao,
                    Detalhamento = produtoSolucao.Detalhamento,
                    UnidadeMedida = unidadeMedida.Descricao,
                    IdNaoConformidade = produtoSolucao.IdNaoConformidade,
                    Quantidade = produtoSolucao.Quantidade,
                    OperacaoEngenharia = produtoSolucao.OperacaoEngenharia
                })
                .Where(entity => entity.IdNaoConformidade.Equals(idNaoConformidade))
                .AsNoTracking()
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);

        var totalCount = await query.CountAsync();
        var itens = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .ToListAsync();
        var output = new PagedResultDto<ProdutoNaoConformidadeViewOutput>(totalCount, itens);
        return output;
    }
}