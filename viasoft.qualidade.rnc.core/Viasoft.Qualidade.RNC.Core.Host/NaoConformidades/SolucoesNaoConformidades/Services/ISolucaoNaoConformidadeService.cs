using System;
using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Services;

public interface ISolucaoNaoConformidadeService
{
    Task<SolucaoNaoConformidadeOutput> Get(Guid idNaoConformidade, Guid id);
    Task Update(Guid idNaoConformidade, Guid idSolucaoNaoConformidade, SolucaoNaoConformidadeInput input);
    Task Insert(Guid idNaoConformidade, SolucaoNaoConformidadeInput input);
    Task Remove(Guid idNaoConformidade, Guid idSolucaoNaoConformidade);
}