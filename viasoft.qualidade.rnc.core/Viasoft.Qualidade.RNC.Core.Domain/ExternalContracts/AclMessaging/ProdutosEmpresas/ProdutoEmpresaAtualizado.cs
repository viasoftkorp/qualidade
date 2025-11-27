using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.AclMessaging.ProdutosEmpresas.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.AclMessaging.ProdutosEmpresas;

[Endpoint("Viasoft.Qualidade.RNC.Core.ProdutoEmpresaAtualizado")]
public class ProdutoEmpresaAtualizado : IMessage
{
    public ProdutoEmpresaDto ProdutoEmpresa { get; set; }

    public ProdutoEmpresaAtualizado()
    {
    }
}