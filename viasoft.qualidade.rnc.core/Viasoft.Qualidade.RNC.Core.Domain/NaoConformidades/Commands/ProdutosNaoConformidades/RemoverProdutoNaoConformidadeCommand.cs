using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ProdutosNaoConformidades;

public class RemoverProdutoNaoConformidadeCommand
{
    public RemoverProdutoNaoConformidadeCommand()
    {
    }
    public Guid Id { get; set; }
    public RemoverProdutoNaoConformidadeCommand(Guid id)
    {
        Id = id;
    }
}