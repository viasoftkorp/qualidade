using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.ServicosNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Produtos.Services;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Recursos.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Handlers;

public class ServicosNaoConformidadesHandler : IHandleMessages<ServicoNaoConformidadeInserido>,
    IHandleMessages<ServicoNaoConformidadeAtualizado>
{
    private readonly IProdutoService _produtoService;
    private readonly IRecursoService _recursoService;

    public ServicosNaoConformidadesHandler(IProdutoService produtoService, IRecursoService recursoService)
    {
        _produtoService = produtoService;
        _recursoService = recursoService;
    }

    public async Task Handle(ServicoNaoConformidadeInserido message)
    {
        if (message.ServicoNaoConformidade.ServicoNaoConformidade.IdProduto.HasValue)
        {
            await _produtoService.InserirSeNaoCadastrado(message.ServicoNaoConformidade.ServicoNaoConformidade.IdProduto.Value);
        }

        await _recursoService.InserirSeNaoCadastrado(message.ServicoNaoConformidade.ServicoNaoConformidade.IdRecurso);

    }
    public async Task Handle(ServicoNaoConformidadeAtualizado message)
    {
        if (message.Command.ServicoNaoConformidade.IdProduto.HasValue)
        {
            await _produtoService.InserirSeNaoCadastrado(message.Command.ServicoNaoConformidade.IdProduto.Value);
        }
        
        await _recursoService.InserirSeNaoCadastrado(message.Command.ServicoNaoConformidade.IdRecurso);

    }
}