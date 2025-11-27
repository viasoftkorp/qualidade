using System;
using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Services;

public interface IAcaoPreventivaNaoConformidadeService
{
    Task<AcaoPreventivaNaoConformidadeOutput> Get(Guid idNaoConformidade, Guid id);
    Task Update(Guid idNaoConformidade, Guid idAcaoPreventivaNaoConformidade,AcaoPreventivaNaoConformidadeInput input);
    Task Insert (Guid idNaoConformidade, AcaoPreventivaNaoConformidadeInput input);
    Task Remove(Guid idNaoConformidade, Guid idAcaoPreventivaNaoConformidade);
   
}