using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Dtos;

public class ProdutoOutput
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public string Codigo { get; set; }
    public Guid? IdCategoria { get; set; }

    public ProdutoOutput()
    {
        
    }

    public ProdutoOutput(ProductOutput productOutput)
    {
        Id = productOutput.Id;
        Descricao = productOutput.Description;
        Codigo = productOutput.Code;
        IdCategoria = productOutput.CategoryId;
    }
}