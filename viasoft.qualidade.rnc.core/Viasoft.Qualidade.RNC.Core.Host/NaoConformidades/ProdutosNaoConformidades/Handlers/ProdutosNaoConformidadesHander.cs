using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.ProdutosNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Produtos.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Handlers;

public class ProdutosNaoConformidadesHander : IHandleMessages<ProdutoNaoConformidadeInserido>,
    IHandleMessages<ProdutoNaoConformidadeAtualizado>
{
    private readonly IProdutoService _produtoService;

    public ProdutosNaoConformidadesHander(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }
    
    public async Task Handle(ProdutoNaoConformidadeInserido message)
    {
        await _produtoService.InserirSeNaoCadastrado(message.Command.ProdutoNaoConformidade.IdProduto);
    }

    public async Task Handle(ProdutoNaoConformidadeAtualizado message)
    {
        await _produtoService.InserirSeNaoCadastrado(message.Command.ProdutoNaoConformidade.IdProduto);
    }
}