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
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Recursos;
using Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Services;

public class ServicoNaoConformidadeViewService : IServicoNaoConformidadeViewService, ITransientDependency
{
    private readonly IRepository<ServicoNaoConformidade> _ServicoNaoConformidades;
    private readonly IRepository<Produto> _produtos;
    private readonly IRepository<Recurso> _recursos;
    private readonly ICurrentCompany _currentCompany;

    public ServicoNaoConformidadeViewService(
        IRepository<ServicoNaoConformidade> ServicoNaoConformidades,
        IRepository<Produto> produtos, IRepository<Recurso> recursos,
        ICurrentCompany currentCompany)
    {
        _ServicoNaoConformidades = ServicoNaoConformidades;
        _produtos = produtos;
        _recursos = recursos;
        _currentCompany = currentCompany;
    }

    public async Task<PagedResultDto<ServicoNaoConformidadeViewOutput>> GetListView(Guid idNaoConformidade, PagedFilteredAndSortedRequestInput input)
    {
        var query = (from servicosSolucoes in _ServicoNaoConformidades
                where servicosSolucoes.CompanyId == _currentCompany.Id
                join produto in _produtos
                        on servicosSolucoes.IdProduto equals produto.Id into produtoJoinedTable
                from produto in produtoJoinedTable.DefaultIfEmpty()
                    join recurso in _recursos
                            on servicosSolucoes.IdRecurso equals recurso.Id into recursoJoinedTable
                from recurso in recursoJoinedTable.DefaultIfEmpty()
                    select new ServicoNaoConformidadeViewOutput
                {
                    Id = servicosSolucoes.Id,
                    IdProduto = servicosSolucoes.IdProduto,
                    IdNaoConformidade = servicosSolucoes.IdNaoConformidade,
                    Quantidade = servicosSolucoes.Quantidade,
                    Horas = servicosSolucoes.Horas,
                    Minutos = servicosSolucoes.Minutos,
                    IdRecurso = servicosSolucoes.IdRecurso,
                    DescricaoRecurso = recurso.Descricao,
                    OperacaoEngenharia = servicosSolucoes.OperacaoEngenharia,
                    Detalhamento = servicosSolucoes.Detalhamento,
                    Codigo = produto.Codigo,
                    Descricao = produto.Descricao
                })
            .Where(entity => entity.IdNaoConformidade.Equals(idNaoConformidade))
            .AsNoTracking()
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);

        var totalCount = await query.CountAsync();
        var itens = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .ToListAsync();
        var output = new PagedResultDto<ServicoNaoConformidadeViewOutput>(totalCount, itens);
        return output;
    }
}