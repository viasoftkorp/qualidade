using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Servicos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Solucoes.Services;

public interface ISolucaoService
{
    Task<SolucaoOutput> Get(Guid id);
    Task<PagedResultDto<SolucaoOutput>> GetList(PagedFilteredAndSortedRequestInput input);
    Task<PagedResultDto<SolucaoViewOutput>> GetViewList(PagedFilteredAndSortedRequestInput input);
    Task<ValidationResult> Create(SolucaoInput input);
    Task<ValidationResult> Update(Guid id, SolucaoInput input);
    Task<ValidationResult> Delete(Guid id);
    Task<ValidationResult> ChangeStatus(Guid id, bool isAtivo);
    Task<ValidationResult> AddProduto(ProdutoSolucaoInput input);
    Task<ValidationResult> UpdateProduto(Guid id, ProdutoSolucaoInput input);
    Task<ValidationResult> DeleteProduto(Guid id);

    Task<PagedResultDto<ProdutoSolucaoViewOutput>> GetProdutoSolucaoList(PagedFilteredAndSortedRequestInput input,
        Guid id);
    Task<ProdutoSolucaoOutput> GetProdutoSolucaoView(Guid id);
    Task<ServicoValidationResult> AddServico(ServicoSolucaoInput input);
    Task<ServicoValidationResult> UpdateServico(Guid id, ServicoSolucaoInput input);
    Task<ValidationResult> DeleteServico(Guid id);
    Task<ServicoSolucaoOutput> GetServicoSolucaoView(Guid id);

    Task<PagedResultDto<ServicoSolucaoViewOutput>> GetServicoSolucaoList(PagedFilteredAndSortedRequestInput input,
        Guid id);
}