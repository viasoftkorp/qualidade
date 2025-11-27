using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Services;

public interface ISolucaoProvider
{
    Task<SolucaoOutput> Get(Guid id);
    Task<PagedResultDto<SolucaoOutput>> GetList(PagedFilteredAndSortedRequestInput input);
    Task<HttpResponseMessage> GetViewList(PagedFilteredAndSortedRequestInput input);
    Task<HttpResponseMessage> Create(SolucaoInput input);
    Task<HttpResponseMessage> Update(Guid id, SolucaoInput input);
    Task<HttpResponseMessage> Delete(Guid id);
    Task<HttpResponseMessage> Inativar(Guid id);
    Task<HttpResponseMessage> Ativar(Guid id);
    Task<ProdutoSolucaoViewOutput> GetProdutoSolucaoView(Guid id, Guid idSolucao);
    Task<HttpResponseMessage> AddProduto(ProdutoSolucaoInput input, Guid idSolucao);
    Task<HttpResponseMessage> UpdateProduto(Guid id, ProdutoSolucaoInput input, Guid idSolucao);
    Task DeleteProduto(Guid id, Guid idSolucao);
    Task<ServicoSolucaoViewOutput> GetServicoSolucaoView(Guid id, Guid idSolucao);
    Task<HttpResponseMessage> AddServico(ServicoSolucaoInput input, Guid idSolucao);
    Task<HttpResponseMessage> UpdateServico(Guid id, ServicoSolucaoInput input, Guid idSolucao);
    Task DeleteServico(Guid id, Guid idSolucao);

    Task<PagedResultDto<ProdutoSolucaoViewOutput>> GetProdutoSolucaoList(PagedFilteredAndSortedRequestInput input,
        Guid idSolucao);

    Task<PagedResultDto<ServicoSolucaoViewOutput>> GetServicoSolucaoList(PagedFilteredAndSortedRequestInput input,
        Guid idSolucao);
}