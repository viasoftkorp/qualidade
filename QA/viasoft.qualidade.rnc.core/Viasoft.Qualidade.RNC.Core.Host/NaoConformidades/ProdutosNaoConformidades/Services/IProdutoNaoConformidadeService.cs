using System;
using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Services;

public interface IProdutoNaoConformidadeService
{
    Task<ProdutoNaoConformidadeOutput> Get(Guid idNaoConformidade, Guid id);
    Task Update(Guid idNaoConformidade, Guid idProdutoNaoConformidade, ProdutoNaoConformidadeInput input);
    Task Insert(Guid idNaoConformidade, ProdutoNaoConformidadeInput input);
    Task Remove(Guid idNaoConformidade, Guid idProdutoNaoConformidade);
}