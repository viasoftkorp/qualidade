using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Events.ServicoSolucoes;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Produtos.Services;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Recursos.Services;


namespace Viasoft.Qualidade.RNC.Core.Host.Solucoes.Handlers;

public class ServicoSolucaoHandler : IHandleMessages<ServicoSolucaoCreated>, IHandleMessages<ServicoSolucaoUpdated>
{
    private readonly IProdutoService _produtoService;
    private readonly IRecursoService _recursoService;

    public ServicoSolucaoHandler(IProdutoService produtoService, IRecursoService recursoService)
    {
        _produtoService = produtoService;
        _recursoService = recursoService;
    }


    public async Task Handle(ServicoSolucaoCreated message)
    {
        if (message.IdProduto.HasValue)
        {
            await _produtoService.InserirSeNaoCadastrado(message.IdProduto.Value);
        }

        await _recursoService.InserirSeNaoCadastrado(message.IdRecurso);
    }

    public async Task Handle(ServicoSolucaoUpdated message)
    {
        if (message.IdProduto.HasValue)
        {
            await _produtoService.InserirSeNaoCadastrado(message.IdProduto.Value);
        }
        
        await _recursoService.InserirSeNaoCadastrado(message.IdRecurso);
    }
}