using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Events.ProdutoSolucoes;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Produtos.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.Solucoes.Handlers;

public class ProdutoSolucaoHandler : IHandleMessages<ProdutoSolucaoCreated>, IHandleMessages<ProdutoSolucaoUpdated>
{
    private readonly IProdutoService _produtoService;

    public ProdutoSolucaoHandler(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    public async Task Handle(ProdutoSolucaoCreated message)
    {
        await _produtoService.InserirSeNaoCadastrado(message.IdProduto);
    }

    public async Task Handle(ProdutoSolucaoUpdated message)
    {
        await _produtoService.InserirSeNaoCadastrado(message.IdProduto);
    }
}