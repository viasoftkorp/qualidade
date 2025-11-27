using System;
using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Services;

public interface ICausaNaoConformidadeService
{
    Task<CausaNaoConformidadeOutput> Get(Guid idNaoConformidade, Guid id);
    Task Update(Guid idNaoConformidade, Guid idCausaNaoConformidade,CausaNaoConformidadeInput input);
    Task Insert (Guid idNaoConformidade, CausaNaoConformidadeInput input);
    Task Remove(Guid idNaoConformidade, Guid idCausaNaoConformidade);
}