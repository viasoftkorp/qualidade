using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.AclMessaging.ProdutosEmpresas.Dtos;

public class ProdutoEmpresaDto
{
    public Guid Id { get; set; }
    public Guid IdProduto { get; set; }
    public Guid IdCategoria { get; set; }
    public Guid IdEmpresa { get; set; }

    public ProdutoEmpresaDto()
    {
    }
}