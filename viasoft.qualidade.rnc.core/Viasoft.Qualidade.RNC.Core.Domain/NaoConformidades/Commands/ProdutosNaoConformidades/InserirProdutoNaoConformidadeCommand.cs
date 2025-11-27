using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.ProdutosNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ProdutosNaoConformidades;

public class InserirProdutoNaoConformidadeCommand 
{
    public ProdutoNaoConformidadeModel ProdutoNaoConformidade { get; set; }
    public InserirProdutoNaoConformidadeCommand()
    {
    }

    public InserirProdutoNaoConformidadeCommand(IProdutoNaoConformidadeModel model)
    {
        ProdutoNaoConformidade = new ProdutoNaoConformidadeModel(model);
    }
}