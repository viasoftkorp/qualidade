using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.AcaoPreventivaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ConclusaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.DefeitoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Services;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ProdutoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ReclamacaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.SolucaoNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;

public class NaoConformidadeRepository : INaoConformidadeRepository, ITransientDependency
{
    private bool RetornarOperacoes { get; set; }
    private readonly IRepository<NaoConformidade> _naoConformidades;
    private readonly IRepository<AcaoPreventivaNaoConformidade> _acaoPreventivaNaoConformidades;
    private readonly IRepository<CausaNaoConformidade> _causaNaoConformidades;
    private readonly IRepository<DefeitoNaoConformidade> _defeitoNaoConformidades;
    private readonly IRepository<SolucaoNaoConformidade> _solucaoNaoConformidades;
    private readonly IRepository<ProdutoNaoConformidade> _produtoNaoConformidades;
    private readonly IRepository<ServicoNaoConformidade> _servicoNaoConformidades;
    private readonly IRepository<CentroCustoCausaNaoConformidade> _centroCustoCausaNaoConformidades;
    private readonly IRepository<ConclusaoNaoConformidade> _conclusaoNaoConformidades;
    private readonly IRepository<ReclamacaoNaoConformidade> _reclamacaoNaoConformidades;
    private readonly IRepository<ImplementacaoEvitarReincidenciaNaoConformidade> _implementacaoEvitarReincidenciaNaoConformidades;
    private readonly IGeracaoCodigoService _geracaoCodigoService;
    private readonly IRepository<OrdemRetrabalhoNaoConformidade> _ordemRetrabalhoNaoConformidades;

    public NaoConformidadeRepository(IRepository<NaoConformidade> naoConformidades,
        IRepository<AcaoPreventivaNaoConformidade> acaoPreventivaNaoConformidades,
        IRepository<CausaNaoConformidade> causaNaoConformidades,
        IRepository<DefeitoNaoConformidade> defeitoNaoConformidades,
        IRepository<SolucaoNaoConformidade> solucaoNaoConformidades,
        IRepository<ProdutoNaoConformidade> produtoNaoConformidades,
        IRepository<ServicoNaoConformidade> servicoNaoConformidades,
        IRepository<CentroCustoCausaNaoConformidade> centroCustoCausaNaoConformidades,
        IRepository<ConclusaoNaoConformidade> conclusaoNaoConformidades,
        IRepository<ReclamacaoNaoConformidade> reclamacaoNaoConformidades,
        IRepository<ImplementacaoEvitarReincidenciaNaoConformidade> implementacaoEvitarReincidenciaNaoConformidades,
        IGeracaoCodigoService geracaoCodigoService,
        IRepository<OrdemRetrabalhoNaoConformidade> ordemRetrabalhoNaoConformidades)
    {
        _naoConformidades = naoConformidades;
        _acaoPreventivaNaoConformidades = acaoPreventivaNaoConformidades;
        _causaNaoConformidades = causaNaoConformidades;
        _defeitoNaoConformidades = defeitoNaoConformidades;
        _solucaoNaoConformidades = solucaoNaoConformidades;
        _produtoNaoConformidades = produtoNaoConformidades;
        _servicoNaoConformidades = servicoNaoConformidades;
        _centroCustoCausaNaoConformidades = centroCustoCausaNaoConformidades;
        _conclusaoNaoConformidades = conclusaoNaoConformidades;
        _reclamacaoNaoConformidades = reclamacaoNaoConformidades;
        _implementacaoEvitarReincidenciaNaoConformidades = implementacaoEvitarReincidenciaNaoConformidades;
        _geracaoCodigoService = geracaoCodigoService;
        _ordemRetrabalhoNaoConformidades = ordemRetrabalhoNaoConformidades;
    }

    public INaoConformidadeRepository Operacoes()
    {
        RetornarOperacoes = true;
        return this;
    }

    public async Task<AgregacaoNaoConformidade> Get(Guid id)
    {
        var query = _naoConformidades.AsQueryable();
        if (RetornarOperacoes)
        {
            query = query.Include(e => e.OperacaoRetrabalho);
            query = query.Include(e => e.OperacaoRetrabalho.Operacoes);
        }
        var naoConformidade = await query
            .Where(naoConformidade => naoConformidade.Id.Equals(id))
            .Select(naoConformidade => new AgregacaoNaoConformidade(
                naoConformidade,
                _acaoPreventivaNaoConformidades
                    .Where(acaoPreventiva => acaoPreventiva.IdNaoConformidade.Equals(naoConformidade.Id)).ToList(),
                _causaNaoConformidades.Where(causa => causa.IdNaoConformidade.Equals(naoConformidade.Id)).ToList(),
                _defeitoNaoConformidades.Where(defeito => defeito.IdNaoConformidade.Equals(naoConformidade.Id))
                    .ToList(),
                _solucaoNaoConformidades.Where(solucao => solucao.IdNaoConformidade.Equals(naoConformidade.Id))
                    .ToList(),
                _produtoNaoConformidades
                    .Where(produtoSolucao => produtoSolucao.IdNaoConformidade.Equals(naoConformidade.Id)).ToList(),
                _servicoNaoConformidades
                    .Where(servicoSolucao => servicoSolucao.IdNaoConformidade.Equals(naoConformidade.Id)).ToList(),
                _centroCustoCausaNaoConformidades
                    .Where(centroCusto => centroCusto.IdNaoConformidade.Equals(naoConformidade.Id)).ToList(),
                _conclusaoNaoConformidades.First(conclusao => conclusao.IdNaoConformidade.Equals(naoConformidade.Id)),
                _reclamacaoNaoConformidades.First(reclamacao =>
                    reclamacao.IdNaoConformidade.Equals(naoConformidade.Id)),
                _ordemRetrabalhoNaoConformidades.First(e => e.IdNaoConformidade.Equals(naoConformidade.Id)),
                _implementacaoEvitarReincidenciaNaoConformidades
                    .Where(e => e.IdNaoConformidade == naoConformidade.Id)
                    .ToList()

            ))
            .FirstOrDefaultAsync();

        return naoConformidade;
    }

    public async Task<List<AgregacaoNaoConformidade>> GetList(List<NaoConformidade> naoConformidades)
    {
        var idsNaoConformidade = naoConformidades
            .Select(entity => entity.Id)
            .ToList();
        var acoesPreventivas = await _acaoPreventivaNaoConformidades
            .Where(entity => idsNaoConformidade.Contains(entity.IdNaoConformidade))
            .ToListAsync();
        var causas = await _causaNaoConformidades
            .Where(entity => idsNaoConformidade.Contains(entity.IdNaoConformidade))
            .ToListAsync();
        var defeitos = await _defeitoNaoConformidades
            .Where(entity => idsNaoConformidade.Contains(entity.IdNaoConformidade))
            .ToListAsync();
        var solucoes = await _solucaoNaoConformidades
            .Where(entity => idsNaoConformidade.Contains(entity.IdNaoConformidade))
            .ToListAsync();
        var produtosSolucoes = await _produtoNaoConformidades
            .Where(entity => idsNaoConformidade.Contains(entity.IdNaoConformidade))
            .ToListAsync();
        var sevicosSolucoes = await _servicoNaoConformidades
            .Where(entity => idsNaoConformidade.Contains(entity.IdNaoConformidade))
            .ToListAsync();
        var centroCustos = await _centroCustoCausaNaoConformidades
            .Where(entity => idsNaoConformidade.Contains(entity.IdNaoConformidade))
            .ToListAsync();
        var reclamacaoClientes = await _reclamacaoNaoConformidades
            .Where(entity => idsNaoConformidade.Contains(entity.IdNaoConformidade))
            .ToListAsync();
        var conclusao = await _conclusaoNaoConformidades
            .Where(entity => idsNaoConformidade.Contains(entity.IdNaoConformidade))
            .ToListAsync();
        var implementacaoEvitarReincidenciaNaoConformidades = await _implementacaoEvitarReincidenciaNaoConformidades
            .Where(entity => idsNaoConformidade.Contains(entity.IdNaoConformidade))
            .ToListAsync();
        var ordemRetrabalhoNaoConformidade = await _ordemRetrabalhoNaoConformidades
            .Where(entity => idsNaoConformidade.Contains(entity.IdNaoConformidade))
            .ToListAsync();
        
        var dicionarioNaoConformidades = await _naoConformidades
            .Where(entity => idsNaoConformidade.Contains(entity.Id))
            .ToDictionaryAsync(entity => entity.Id);

        var dicionarioAcoesPreventivas = acoesPreventivas
            .GroupBy(naoConformidade => naoConformidade.IdNaoConformidade, (idNaoConformidade, entity) => new
            {
                IdNaoConformidade = idNaoConformidade,
                Entity = entity.ToList()
            })
            .ToDictionary(grouping => grouping.IdNaoConformidade,
                grouping => grouping.Entity);
        var dicionarioCausas = causas
            .GroupBy(naoConformidade => naoConformidade.IdNaoConformidade, (idNaoConformidade, entity) => new
            {
                IdNaoConformidade = idNaoConformidade,
                Entity = entity.ToList()
            })
            .ToDictionary(grouping => grouping.IdNaoConformidade,
                grouping => grouping.Entity);

        var dicionarioDefeitos = defeitos
            .GroupBy(naoConformidade => naoConformidade.IdNaoConformidade, (idNaoConformidade, entity) => new
            {
                IdNaoConformidade = idNaoConformidade,
                Entity = entity.ToList()
            })
            .ToDictionary(grouping => grouping.IdNaoConformidade,
                grouping => grouping.Entity);

        var dicionarioSolucoes = solucoes
            .GroupBy(naoConformidade => naoConformidade.IdNaoConformidade, (idNaoConformidade, entity) => new
            {
                IdNaoConformidade = idNaoConformidade,
                Entity = entity.ToList()
            })
            .ToDictionary(grouping => grouping.IdNaoConformidade,
                grouping => grouping.Entity);

        var dicionarioProdutosSolucoes = produtosSolucoes
            .GroupBy(naoConformidade => naoConformidade.IdNaoConformidade, (idNaoConformidade, entity) => new
            {
                IdNaoConformidade = idNaoConformidade,
                Entity = entity.ToList()
            })
            .ToDictionary(grouping => grouping.IdNaoConformidade,
                grouping => grouping.Entity);

        var dicionarioServicosSolucoes = sevicosSolucoes
            .GroupBy(naoConformidade => naoConformidade.IdNaoConformidade, (idNaoConformidade, entity) => new
            {
                IdNaoConformidade = idNaoConformidade,
                Entity = entity.ToList()
            })
            .ToDictionary(grouping => grouping.IdNaoConformidade,
                grouping => grouping.Entity);

        var dicionarioimplementacaoEvitarReincidencia = implementacaoEvitarReincidenciaNaoConformidades
            .GroupBy(naoConformidade => naoConformidade.IdNaoConformidade, (idNaoConformidade, entity) => new
            {
                IdNaoConformidade = idNaoConformidade,
                Entity = entity.ToList()
            })
            .ToDictionary(grouping => grouping.IdNaoConformidade,
                grouping => grouping.Entity);
        
        var dicionarioCentroCustosNaoConformidade = centroCustos
            .GroupBy(naoConformidade => naoConformidade.IdNaoConformidade, (idNaoConformidade, entity) => new
            {
                IdNaoConformidade = idNaoConformidade,
                Entity = entity.ToList()
            })
            .ToDictionary(grouping => grouping.IdNaoConformidade,
                grouping => grouping.Entity);
        
        return naoConformidades.Select(naoConformidade => new AgregacaoNaoConformidade(
            dicionarioNaoConformidades[naoConformidade.Id],
            dicionarioAcoesPreventivas.ContainsKey(naoConformidade.Id)
                ? dicionarioAcoesPreventivas[naoConformidade.Id]
                : new List<AcaoPreventivaNaoConformidade>(),
            dicionarioCausas.ContainsKey(naoConformidade.Id)
                ? dicionarioCausas[naoConformidade.Id]
                : new List<CausaNaoConformidade>(),
            dicionarioDefeitos.ContainsKey(naoConformidade.Id)
                ? dicionarioDefeitos[naoConformidade.Id]
                : new List<DefeitoNaoConformidade>(),
            dicionarioSolucoes.ContainsKey(naoConformidade.Id)
                ? dicionarioSolucoes[naoConformidade.Id]
                : new List<SolucaoNaoConformidade>(),
            dicionarioProdutosSolucoes.ContainsKey(naoConformidade.Id)
                ? dicionarioProdutosSolucoes[naoConformidade.Id]
                : new List<ProdutoNaoConformidade>(),
            dicionarioServicosSolucoes.ContainsKey(naoConformidade.Id)
                ? dicionarioServicosSolucoes[naoConformidade.Id]
                : new List<ServicoNaoConformidade>(),
            dicionarioCentroCustosNaoConformidade.ContainsKey(naoConformidade.Id)
                ? dicionarioCentroCustosNaoConformidade[naoConformidade.Id]
                : new List<CentroCustoCausaNaoConformidade>(),
            conclusao.Find(p => p.IdNaoConformidade == naoConformidade.Id),
            reclamacaoClientes.Find(p => p.IdNaoConformidade == naoConformidade.Id),
            ordemRetrabalhoNaoConformidade.Find(e => e.IdNaoConformidade == naoConformidade.Id),
            dicionarioimplementacaoEvitarReincidencia.ContainsKey(naoConformidade.Id)
                ? dicionarioimplementacaoEvitarReincidencia[naoConformidade.Id]
                : new List<ImplementacaoEvitarReincidenciaNaoConformidade>()
            
        )).ToList();
    }

    public async Task Update(UpdateNaoConformidadeInput input)
    {
        if (input.NaoConformidadeAtualizar != null && !input.NaoConformidadeAtualizar.Codigo.HasValue)
        {
            input.NaoConformidadeAtualizar.Codigo = await _geracaoCodigoService.GetCodigoNaoConformidade();
        }

        await _naoConformidades.UpdateAsync(input.NaoConformidadeAtualizar);
        
        if (input.NaoConformidadeRemover != null)
        {
            await _naoConformidades.DeleteAsync(input.NaoConformidadeRemover);
        }

        if (input.ConclusaoAtualizar != null)
        {
            await _conclusaoNaoConformidades.UpdateAsync(input.ConclusaoAtualizar);
        }

        if (input.ConclusaoCriar != null)
        {
            await _conclusaoNaoConformidades.InsertAsync(input.ConclusaoCriar);
        }
        
        if (input.ConclusaoRemover != null)
        {
            await _conclusaoNaoConformidades.DeleteAsync(input.ConclusaoRemover);
        }

        if (input.ReclamacaoAtualizar != null)
        {
            await _reclamacaoNaoConformidades.UpdateAsync(input.ReclamacaoAtualizar);
        }

        if (input.ReclamacaoCriar != null)
        {
            await _reclamacaoNaoConformidades.InsertAsync(input.ReclamacaoCriar);
        }

        foreach (var acao in input.AcoesAtualizar)
        {
            await _acaoPreventivaNaoConformidades.UpdateAsync(acao);
        }

        foreach (var acao in input.AcoesCriar)
        {
            await _acaoPreventivaNaoConformidades.InsertAsync(acao);
        }

        foreach (var acao in input.AcoesRemover)
        {
            await _acaoPreventivaNaoConformidades.DeleteAsync(acao);
        }

        foreach (var causa in input.CausasAtualizar)
        {
            await _causaNaoConformidades.UpdateAsync(causa);
        }

        foreach (var causa in input.CausasCriar)
        {
            await _causaNaoConformidades.InsertAsync(causa);
        }

        foreach (var causa in input.CausasRemover)
        {
            await _causaNaoConformidades.DeleteAsync(causa);
        }

        foreach (var defeito in input.DefeitosAtualizar)
        {
            await _defeitoNaoConformidades.UpdateAsync(defeito);
        }

        foreach (var defeito in input.DefeitosCriar)
        {
            await _defeitoNaoConformidades.InsertAsync(defeito);
        }

        foreach (var defeito in input.DefeitosRemover)
        {
            await _defeitoNaoConformidades.DeleteAsync(defeito);
        }

        foreach (var solucao in input.SolucoesAtualizar)
        {
            await _solucaoNaoConformidades.UpdateAsync(solucao);
        }

        foreach (var solucao in input.SolucoesCriar)
        {
            await _solucaoNaoConformidades.InsertAsync(solucao);
        }

        foreach (var solucao in input.SolucoesRemover)
        {
            await _solucaoNaoConformidades.DeleteAsync(solucao);
        }

        foreach (var produto in input.ProdutosAtualizar)
        {
            await _produtoNaoConformidades.UpdateAsync(produto);
        }

        foreach (var produto in input.ProdutosCriar)
        {
            await _produtoNaoConformidades.InsertAsync(produto);
        }

        foreach (var produto in input.ProdutosRemover)
        {
            await _produtoNaoConformidades.DeleteAsync(produto);
        }

        foreach (var servico in input.ServicosAtualizar)
        {
            await _servicoNaoConformidades.UpdateAsync(servico);
        }

        foreach (var servico in input.ServicosCriar)
        {
            await _servicoNaoConformidades.InsertAsync(servico);
        }

        foreach (var servico in input.ServicosRemover)
        {
            await _servicoNaoConformidades.DeleteAsync(servico);
        }
        
        foreach (var implementacao in input.ImplemetacaoEvitarReincidenciaAAtualizar)
        {
            await _implementacaoEvitarReincidenciaNaoConformidades.UpdateAsync(implementacao);
        }

        foreach (var implementacao in input.ImplemetacaoEvitarReincidenciaACriar)
        {
            await _implementacaoEvitarReincidenciaNaoConformidades.InsertAsync(implementacao);
        }

        foreach (var implementacao in input.ImplemetacaoEvitarReincidenciaARemover)
        {
            await _implementacaoEvitarReincidenciaNaoConformidades.DeleteAsync(implementacao);
        }
        
        foreach (var centroCusto in input.CentroCustoCausaNaoConformidadeCriar)
        {
            await _centroCustoCausaNaoConformidades.InsertAsync(centroCusto);
        }

        foreach (var centroCusto in input.CentroCustoCausaNaoConformidadeRemover)
        {
            await _centroCustoCausaNaoConformidades.DeleteAsync(centroCusto);
        }
    }

    public async Task Create(CreateNaoConformidadeInput input)
    {
        
        if (!input.NaoConformidadeACriar.Incompleta && (!input.NaoConformidadeACriar.Codigo.HasValue || input.NaoConformidadeACriar.Codigo == 0 ))
        {
            input.NaoConformidadeACriar.Codigo = await _geracaoCodigoService.GetCodigoNaoConformidade();
        }
        
        await _naoConformidades.InsertAsync(input.NaoConformidadeACriar);
        
        if (input.ConclusaoACriar != null)
        {
            await _conclusaoNaoConformidades.InsertAsync(input.ConclusaoACriar);
        }

        if (input.ReclamacaoACriar != null)
        {
            await _reclamacaoNaoConformidades.InsertAsync(input.ReclamacaoACriar);
        }

        await _acaoPreventivaNaoConformidades.InsertRangeAsync(input.AcaoPreventivaNaoConformidadesACriar);
        await _causaNaoConformidades.InsertRangeAsync(input.CausaNaoConformidadesACriar);
        await _defeitoNaoConformidades.InsertRangeAsync(input.DefeitoNaoConformidadesACriar);
        await _solucaoNaoConformidades.InsertRangeAsync(input.SolucaoNaoConformidadesACriar);
        await _produtoNaoConformidades.InsertRangeAsync(input.ProdutoNaoConformidadesACriar);
        await _servicoNaoConformidades.InsertRangeAsync(input.ServicoNaoConformidadesACriar);
    }

    public async Task Delete(NaoConformidade naoConformidade, ConclusaoNaoConformidade conclusao,
        ReclamacaoNaoConformidade reclamacao,
        List<AcaoPreventivaNaoConformidade> acaoPreventivaNaoConformidades,
        List<CausaNaoConformidade> causaNaoConformidades,
        List<DefeitoNaoConformidade> defeitoNaoConformidades, List<SolucaoNaoConformidade> solucaoNaoConformidades,
        List<ProdutoNaoConformidade> produtoNaoConformidades,
        List<ServicoNaoConformidade> servicoNaoConformidades)
    {
        
        if (naoConformidade != null)
        {
            await _naoConformidades.DeleteAsync(naoConformidade);
        }

        if (conclusao != null)
        {
            await _conclusaoNaoConformidades.DeleteAsync(conclusao);
        }

        if (reclamacao != null)
        {
            await _reclamacaoNaoConformidades.DeleteAsync(reclamacao);
        }

        foreach (var acao in acaoPreventivaNaoConformidades)
        {
            await _acaoPreventivaNaoConformidades.DeleteAsync(acao);
        }

        foreach (var causa in causaNaoConformidades)
        {
            await _causaNaoConformidades.DeleteAsync(causa);
        }

        foreach (var defeito in defeitoNaoConformidades)
        {
            await _defeitoNaoConformidades.DeleteAsync(defeito);
        }

        foreach (var produtoSolucao in produtoNaoConformidades)
        {
            await _produtoNaoConformidades.DeleteAsync(produtoSolucao);
        }

        foreach (var servicoSolucao in servicoNaoConformidades)
        {
            await _servicoNaoConformidades.DeleteAsync(servicoSolucao);
        }

        foreach (var solucao in solucaoNaoConformidades)
        {
            await _solucaoNaoConformidades.DeleteAsync(solucao);
        }
    }
}
