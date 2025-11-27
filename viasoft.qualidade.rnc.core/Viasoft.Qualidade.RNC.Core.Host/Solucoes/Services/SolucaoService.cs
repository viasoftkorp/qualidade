using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Data.Extensions.Filtering.AdvancedFilter;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Recursos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.UnidadeMedidaProdutos;
using Viasoft.Qualidade.RNC.Core.Domain.SolucaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Events.ProdutoSolucoes;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Events.ServicoSolucoes;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Servicos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Servicos.Services;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Solucoes.Services;

public class SolucaoService : ISolucaoService, ITransientDependency
{
    private readonly IRepository<Solucao> _solucoes;
    private readonly IRepository<ProdutoSolucao> _produtoSolucoes;
    private readonly IRepository<ServicoSolucao> _servicoSolucoes;
    private readonly IRepository<Produto> _produtos;
    private readonly IRepository<UnidadeMedidaProduto> _unidadeMedidaProdutos;
    private readonly IRepository<Recurso> _recursos;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentEnvironment _currentEnvironment;
    private readonly ICurrentTenant _currentTenant;
    private readonly IRepository<Defeito> _defeitos;
    private readonly IRepository<SolucaoNaoConformidade> _solucaoNaoConformidades;
    private readonly IServicoValidatorService _servicoValidatorService;
    private readonly IServiceBus _serviceBus;

    public SolucaoService(IRepository<Solucao> solucao, IRepository<ProdutoSolucao> produtoSolucao,
        IRepository<ServicoSolucao> servicoSolucao, IRepository<Produto> produtos,
        IRepository<UnidadeMedidaProduto> unidadeMedidaProdutos,
        IRepository<Recurso> recursos,
        IServiceBus serviceBus, IDateTimeProvider dateTimeProvider,
        ICurrentEnvironment currentEnvironment, ICurrentTenant currentTenant,
        IRepository<Defeito> defeitos, IRepository<SolucaoNaoConformidade> solucaoNaoConformidades,
        IServicoValidatorService servicoValidatorService)
    {
        _solucoes = solucao;
        _produtoSolucoes = produtoSolucao;
        _servicoSolucoes = servicoSolucao;
        _produtos = produtos;
        _unidadeMedidaProdutos = unidadeMedidaProdutos;
        _recursos = recursos;
        _serviceBus = serviceBus;
        _dateTimeProvider = dateTimeProvider;
        _currentEnvironment = currentEnvironment;
        _currentTenant = currentTenant;
        _defeitos = defeitos;
        _solucaoNaoConformidades = solucaoNaoConformidades;
        _servicoValidatorService = servicoValidatorService;
    }

    public async Task<SolucaoOutput> Get(Guid id)
    {
        var solucao = await _solucoes.FirstOrDefaultAsync(s => s.Id == id);
        if (solucao == null)
        {
            return null;
        }

        var output = new SolucaoOutput(solucao);
        return output;
    }

    public async Task<PagedResultDto<SolucaoOutput>> GetList(PagedFilteredAndSortedRequestInput input)
    {
        var query = _solucoes
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);

        var totalCount = await query.CountAsync();

        var solucao = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .Select(solucao => new SolucaoOutput(solucao))
            .ToListAsync();

        return new PagedResultDto<SolucaoOutput>
        {
            TotalCount = totalCount,
            Items = solucao
        };
    }
    public async Task<PagedResultDto<SolucaoViewOutput>> GetViewList(PagedFilteredAndSortedRequestInput input)
    {
        var query = _solucoes
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);

        var totalCount = await query.CountAsync();

        var solucao = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .Select(solucao => new SolucaoViewOutput(solucao))
            .ToListAsync();

        return new PagedResultDto<SolucaoViewOutput>
        {
            TotalCount = totalCount,
            Items = solucao
        };
    }

    public async Task<ValidationResult> Create(SolucaoInput input)
    {
        var solucao = new Solucao(input);

        await _solucoes.InsertAsync(solucao, true);
        return ValidationResult.Ok;
    }

    public async Task<ValidationResult> Update(Guid id, SolucaoInput input)
    {
        var solucao = await _solucoes.FirstOrDefaultAsync(e => e.Id == id);
        if (solucao == null)
        {
            return ValidationResult.NotFound;
        }

        solucao.Update(input);

        await _solucoes.UpdateAsync(solucao, true);
        return ValidationResult.Ok;
    }

    public async Task<ValidationResult> Delete(Guid id)
    {
        var entidadeEmUsoPorDefeito = await _defeitos.AnyAsync(e => e.IdSolucao == id);
        if (entidadeEmUsoPorDefeito)
        {
            return ValidationResult.EntidadeEmUso;
        }

        var entidadeEmUsoPorNaoConformidade = await _solucaoNaoConformidades.AnyAsync(e => e.IdSolucao == id);
        if (entidadeEmUsoPorNaoConformidade)
        {
            return ValidationResult.EntidadeEmUso;
        }
        
        var solucao = await _solucoes.FirstOrDefaultAsync(e => e.Id == id);
        if (solucao == null)
        {
            return ValidationResult.NotFound;
        }

        await _solucoes.DeleteAsync(solucao, true);

        return ValidationResult.Ok;
    }
    
    public async Task<ValidationResult> ChangeStatus(Guid id, bool isAtivo)
    {
        var entidade = await _solucoes.FindAsync(id);
        entidade.IsAtivo = isAtivo;
        
        await _solucoes.UpdateAsync(entidade, true);
        return ValidationResult.Ok;
    }


    public async Task<ProdutoSolucaoOutput> GetProdutoSolucaoView(Guid id)
    {
        var entity = await _produtoSolucoes.FirstOrDefaultAsync(s => s.Id == id);
        if (entity == null)
        {
            return null;
        }

        var output = new ProdutoSolucaoOutput(entity);
        return output;
    }

    public async Task<ValidationResult> AddProduto(ProdutoSolucaoInput input)
    {
        var produtoSolucao = new ProdutoSolucao(input);
        var produtoSolucaoInserido = await _produtoSolucoes.InsertAsync(produtoSolucao, true);
        await _serviceBus.Publish(new ProdutoSolucaoCreated(produtoSolucaoInserido, Guid.NewGuid(),
            _dateTimeProvider.UtcNow(), _currentTenant.Id, _currentEnvironment.Id.Value));
        return ValidationResult.Ok;
    }

    public async Task<ValidationResult> UpdateProduto(Guid id, ProdutoSolucaoInput input)
    {
        var produtoSolucao = await _produtoSolucoes.FirstOrDefaultAsync(e => e.Id == id);
        if (produtoSolucao == null)
        {
            return ValidationResult.NotFound;
        }

        produtoSolucao.Update(input);

        var produtoSolucaoAlterado = await _produtoSolucoes.UpdateAsync(produtoSolucao, true);
        await _serviceBus.Publish(new ProdutoSolucaoUpdated(produtoSolucaoAlterado, Guid.NewGuid(),
            _dateTimeProvider.UtcNow(), _currentTenant.Id, _currentEnvironment.Id.Value));
        return ValidationResult.Ok;
    }

    public async Task<ValidationResult> DeleteProduto(Guid id)
    {
        var produtoSolucao = await _produtoSolucoes.FirstOrDefaultAsync(e => e.Id == id);
        if (produtoSolucao == null)
        {
            return ValidationResult.NotFound;
        }

        await _produtoSolucoes.DeleteAsync(produtoSolucao, true);
        await _serviceBus.Publish(new ProdutoSolucaoDeleted(produtoSolucao, Guid.NewGuid(),
            _dateTimeProvider.UtcNow(), _currentTenant.Id, _currentEnvironment.Id.Value));
        return ValidationResult.Ok;
    }

    public async Task<ServicoValidationResult> AddServico(ServicoSolucaoInput input)
    {
        var servicoSolucao = new ServicoSolucao(input);
        if (await OperacaoEngenhariaJaUtilizada(input.IdSolucao, input))
        {
            return ServicoValidationResult.OperacaoEngenhariaJaUtilizada;
        }

        var tempoValido = _servicoValidatorService.ValidarTempo(input.Horas, input.Minutos);
        if (!tempoValido)
        {
            return ServicoValidationResult.TempoInvalido;
        }
        var servicoSolucaoInserido = await _servicoSolucoes.InsertAsync(servicoSolucao, true);
        await _serviceBus.Publish(new ServicoSolucaoCreated(servicoSolucaoInserido, Guid.NewGuid(),
            _dateTimeProvider.UtcNow(), _currentTenant.Id, _currentEnvironment.Id.Value));
        return ServicoValidationResult.Ok;
    }

    public async Task<ServicoValidationResult> UpdateServico(Guid id, ServicoSolucaoInput input)
    {
        var servicoSolucao = await _servicoSolucoes.FirstOrDefaultAsync(e => e.Id == id);
        if (servicoSolucao == null)
        {
            return ServicoValidationResult.NotFound;
        }
        
        if (await OperacaoEngenhariaJaUtilizada(input.IdSolucao, input))
        {
            return ServicoValidationResult.OperacaoEngenhariaJaUtilizada;
        }
        
        var tempoValido = _servicoValidatorService.ValidarTempo(input.Horas, input.Minutos);
        if (!tempoValido)
        {
            return ServicoValidationResult.TempoInvalido;
        }

        servicoSolucao.Update(input);

        var servicoSolucaoAlterado = await _servicoSolucoes.UpdateAsync(servicoSolucao, true);
        await _serviceBus.Publish(new ServicoSolucaoUpdated(servicoSolucaoAlterado, Guid.NewGuid(),
            _dateTimeProvider.UtcNow(), _currentTenant.Id, _currentEnvironment.Id.Value));
        return ServicoValidationResult.Ok;
    }

    public async Task<ValidationResult> DeleteServico(Guid id)
    {
        var servicoSolucao = await _servicoSolucoes.FirstOrDefaultAsync(e => e.Id == id);
        if (servicoSolucao == null)
        {
            return ValidationResult.NotFound;
        }

        await _servicoSolucoes.DeleteAsync(servicoSolucao, true);
        await _serviceBus.Publish(new ServicoSolucaoDeleted(servicoSolucao, Guid.NewGuid(),
            _dateTimeProvider.UtcNow(), _currentTenant.Id, _currentEnvironment.Id.Value));
        return ValidationResult.Ok;
    }

    public async Task<PagedResultDto<ProdutoSolucaoViewOutput>> GetProdutoSolucaoList(
        PagedFilteredAndSortedRequestInput input, Guid idSolucao)
    {
        var query = (from produtoSolucao in _produtoSolucoes
                join produtos in _produtos
                    on produtoSolucao.IdProduto equals produtos.Id into produtoJoinedTable
                from produtos in produtoJoinedTable.DefaultIfEmpty()
                join unidadeMedida in _unidadeMedidaProdutos
                    on produtos.IdUnidadeMedida equals unidadeMedida.Id into unidadeJoinedTable
                from unidadeMedida in unidadeJoinedTable.DefaultIfEmpty()
                select new ProdutoSolucaoViewOutput
                {
                    Id = produtoSolucao.Id,
                    IdSolucao = produtoSolucao.IdSolucao,
                    IdProduto = produtoSolucao.IdProduto,
                    Quantidade = produtoSolucao.Quantidade,
                    Descricao = produtos.Descricao,
                    Codigo = produtos.Codigo,
                    UnidadeMedida = unidadeMedida.Descricao,
                    // Alterar a sintaxe para $"{}" vai causar problemas no filtro
                    Produto = produtos.Codigo + " - " + produtos.Descricao,
                    OperacaoEngenharia = produtoSolucao.OperacaoEngenharia
                })
            .Where(p => p.IdSolucao == idSolucao)
            .AsNoTracking()
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);

        var totalCount = await query.CountAsync();

        var solucaoProdutoList = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .ToListAsync();

        return new PagedResultDto<ProdutoSolucaoViewOutput>
        {
            TotalCount = totalCount,
            Items = solucaoProdutoList
        };
    }

    public async Task<PagedResultDto<ServicoSolucaoViewOutput>> GetServicoSolucaoList(
        PagedFilteredAndSortedRequestInput input, Guid idSolucao)
    {
        var query = (from servicos in _servicoSolucoes
                join produtos in _produtos
                    on servicos.IdProduto equals produtos.Id into produtoJoinedTable
                from produtos in produtoJoinedTable.DefaultIfEmpty()
                join recurso in _recursos
                    on servicos.IdRecurso equals recurso.Id into recursoJoinedTable
                from recurso in recursoJoinedTable.DefaultIfEmpty()
                select new ServicoSolucaoViewOutput
                {
                    Id = servicos.Id,
                    IdSolucao = servicos.IdSolucao,
                    IdProduto = servicos.IdProduto,
                    Quantidade = servicos.Quantidade,
                    Descricao = produtos.Descricao,
                    Codigo = produtos.Codigo,
                    Horas = servicos.Horas,
                    Minutos = servicos.Minutos,
                    IdRecurso = servicos.IdRecurso,
                    Recurso = recurso.Descricao,
                    OperacaoEngenharia = servicos.OperacaoEngenharia,
                    Produto = $"{produtos.Codigo} - {produtos.Descricao}",
                })
            .Where(p => p.IdSolucao == idSolucao)
            .AsNoTracking()
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);

        var totalCount = await query.CountAsync();

        var entity = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .ToListAsync();

        return new PagedResultDto<ServicoSolucaoViewOutput>
        {
            TotalCount = totalCount,
            Items = entity
        };
    }

    public async Task<ServicoSolucaoOutput> GetServicoSolucaoView(Guid id)
    {
        var servicoSolucaoView = await _servicoSolucoes.FirstOrDefaultAsync(s => s.Id == id);
        if (servicoSolucaoView == null)
        {
            return null;
        }

        var output = new ServicoSolucaoOutput(servicoSolucaoView);
        return output;
    }
    private async Task <bool> OperacaoEngenhariaJaUtilizada(Guid idSolucao, ServicoSolucaoInput input)
    {
        var servicos = await _servicoSolucoes.Where(e => e.IdSolucao == idSolucao).ToListAsync();
        var operacaoEngenhariaJaUtilizada = servicos.Any(e => e.Id != input.Id && e.OperacaoEngenharia == input.OperacaoEngenharia);
        return operacaoEngenhariaJaUtilizada;
    }
}