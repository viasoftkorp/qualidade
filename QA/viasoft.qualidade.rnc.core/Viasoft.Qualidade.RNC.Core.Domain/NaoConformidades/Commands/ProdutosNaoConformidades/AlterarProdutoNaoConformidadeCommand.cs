using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.ProdutosNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ProdutosNaoConformidades;

public class AlterarProdutoNaoConformidadeCommand
{
    public ProdutoNaoConformidadeModel ProdutoNaoConformidade { get; set; }
    public AlterarProdutoNaoConformidadeCommand()
    {
    }

    public AlterarProdutoNaoConformidadeCommand(IProdutoNaoConformidadeModel model)
    {
        ProdutoNaoConformidade = new ProdutoNaoConformidadeModel(model);
    }
}