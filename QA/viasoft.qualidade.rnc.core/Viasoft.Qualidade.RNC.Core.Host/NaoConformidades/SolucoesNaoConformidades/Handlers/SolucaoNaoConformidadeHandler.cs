using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.SolucoesNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Usuarios.Services;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Services;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Handlers;

public class SolucaoNaoConformidadeHandler : 
    IHandleMessages<SolucaoNaoConformidadeInserida>,
    IHandleMessages<SolucaoNaoConformidadeAtualizada>
{
    private readonly IProdutoNaoConformidadeService _produtoNaoConformidadeService;
    private readonly IServicoNaoConformidadeservice _servicoNaoConformidadeservice;
    private readonly IRepository<ProdutoSolucao> _produtoSolucoes;
    private readonly IRepository<ServicoSolucao> _servicoSolucoes;
    private readonly IUsuarioService _usuarioService;

    public SolucaoNaoConformidadeHandler(IProdutoNaoConformidadeService produtoNaoConformidadeService,
        IServicoNaoConformidadeservice servicoNaoConformidadeservice,
        IRepository<ProdutoSolucao> produtoSolucoes, IRepository<ServicoSolucao> servicoSolucoes,
        IUsuarioService usuarioService)
    {
        _produtoNaoConformidadeService = produtoNaoConformidadeService;
        _servicoNaoConformidadeservice = servicoNaoConformidadeservice;
        _produtoSolucoes = produtoSolucoes;
        _servicoSolucoes = servicoSolucoes;
        _usuarioService = usuarioService;
    }
    public async Task Handle(SolucaoNaoConformidadeInserida message)
    {
        if (message.Command.SolucaoNaoConformidade.IdResponsavel.HasValue)
        {
            await _usuarioService.InserirSeNaoCadastrado(message.Command.SolucaoNaoConformidade.IdResponsavel.Value);
        }

        if (message.Command.SolucaoNaoConformidade.IdAuditor.HasValue)
        {
            await _usuarioService.InserirSeNaoCadastrado(message.Command.SolucaoNaoConformidade.IdAuditor.Value);
        }

        var produtosSolucoes = await _produtoSolucoes
            .Where(entity => entity.IdSolucao.Equals(message.Command.SolucaoNaoConformidade.IdSolucao))
            .Select(entity => new ProdutoSolucao(entity)).ToListAsync();
        
        if (produtosSolucoes.Count > 0)
        {
            foreach (var produtoSolucao in produtosSolucoes)
            {
                var input = new ProdutoNaoConformidadeInput
                {
                    Id = Guid.NewGuid(),
                    IdProduto = produtoSolucao.IdProduto,
                    IdNaoConformidade = message.Command.SolucaoNaoConformidade.IdNaoConformidade,
                    Detalhamento = message.Command.SolucaoNaoConformidade.Detalhamento,
                    Quantidade = produtoSolucao.Quantidade,
                    OperacaoEngenharia = produtoSolucao.OperacaoEngenharia
                };
                await _produtoNaoConformidadeService.Insert(message.Command.SolucaoNaoConformidade.IdNaoConformidade, input);
            }
        }

        var servicosSolucoes = await _servicoSolucoes
            .Where(entity => entity.IdSolucao.Equals(message.Command.SolucaoNaoConformidade.IdSolucao))
            .Select(entity => new ServicoSolucao(entity)).ToListAsync();

        if (servicosSolucoes.Count > 0)
        {
            foreach (var servicoSolucao in servicosSolucoes)
            {
                var input = new ServicoNaoConformidadeInput
                {
                    Id = Guid.NewGuid(),
                    IdProduto = servicoSolucao.IdProduto,
                    Quantidade = servicoSolucao.Quantidade,
                    Horas = servicoSolucao.Horas,
                    Minutos = servicoSolucao.Minutos,
                    IdRecurso = servicoSolucao.IdRecurso,
                    OperacaoEngenharia = servicoSolucao.OperacaoEngenharia,
                    Detalhamento = message.Command.SolucaoNaoConformidade.Detalhamento,
                    IdNaoConformidade = message.Command.SolucaoNaoConformidade.IdNaoConformidade,
                };
                await _servicoNaoConformidadeservice.Insert(message.Command.SolucaoNaoConformidade.IdNaoConformidade, input);
            }
        }
    }

    public async Task Handle(SolucaoNaoConformidadeAtualizada message)
    {
        if (message.Command.SolucaoNaoConformidade.IdResponsavel.HasValue)
        {
            await _usuarioService.InserirSeNaoCadastrado(message.Command.SolucaoNaoConformidade.IdResponsavel.Value);
        }

        if (message.Command.SolucaoNaoConformidade.IdAuditor.HasValue)
        {
            await _usuarioService.InserirSeNaoCadastrado(message.Command.SolucaoNaoConformidade.IdAuditor.Value);
        }
    }
}