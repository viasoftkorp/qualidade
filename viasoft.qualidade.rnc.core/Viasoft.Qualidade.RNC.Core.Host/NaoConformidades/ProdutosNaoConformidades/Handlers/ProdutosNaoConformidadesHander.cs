using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Core.AmbientData;
using Viasoft.Core.AmbientData.Extensions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.ProdutosNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Produtos.Services;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.ProdutosEmpresas.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Handlers;

public class ProdutosNaoConformidadesHander : IHandleMessages<ProdutoNaoConformidadeInserido>,
    IHandleMessages<ProdutoNaoConformidadeAtualizado>
{
    private readonly IProdutoService _produtoService;
    private readonly IProdutoEmpresaService _produtoEmpresaService;
    private readonly IAmbientData _ambientData;

    public ProdutosNaoConformidadesHander(IProdutoService produtoService, IProdutoEmpresaService produtoEmpresaService, IAmbientData ambientData)
    {
        _produtoService = produtoService;
        _produtoEmpresaService = produtoEmpresaService;
        _ambientData = ambientData;
    }
    
    public async Task Handle(ProdutoNaoConformidadeInserido message)
    {
        var idProduto = message.Command.ProdutoNaoConformidade.IdProduto;

        await _produtoService.InserirSeNaoCadastrado(idProduto);
        await _produtoEmpresaService.InserirSeNaoCadastrado(idProduto,
            produtoEmpresa => produtoEmpresa.IdProduto == idProduto && produtoEmpresa.IdEmpresa == _ambientData.GetCompanyId());
    }

    public async Task Handle(ProdutoNaoConformidadeAtualizado message)
    {
        var idProduto = message.Command.ProdutoNaoConformidade.IdProduto;

        await _produtoService.InserirSeNaoCadastrado(idProduto);
        await _produtoEmpresaService.InserirSeNaoCadastrado(idProduto,
            produtoEmpresa => produtoEmpresa.IdProduto == idProduto && produtoEmpresa.IdEmpresa == _ambientData.GetCompanyId());
    }
}