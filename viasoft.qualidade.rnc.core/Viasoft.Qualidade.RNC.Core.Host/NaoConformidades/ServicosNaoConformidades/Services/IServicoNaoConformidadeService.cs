using System;
using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Servicos.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Services;

public interface IServicoNaoConformidadeservice
{
    Task<ServicoNaoConformidadeOutput> Get(Guid idNaoConformidade, Guid id);
    Task<ServicoValidationResult> Update(Guid idNaoConformidade, Guid idServicoSolucaoNaoConformidade, ServicoNaoConformidadeInput input);
    Task<ServicoValidationResult> Insert(Guid idNaoConformidade, ServicoNaoConformidadeInput input);
    Task Remove(Guid idNaoConformidade, Guid idServicoSolucaoNaoConformidade);
}