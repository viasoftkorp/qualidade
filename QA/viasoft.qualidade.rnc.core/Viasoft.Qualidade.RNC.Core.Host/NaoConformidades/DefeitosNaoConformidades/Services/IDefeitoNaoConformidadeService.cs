using System;
using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Services;

public interface IDefeitoNaoConformidadeService
{
    Task<DefeitoNaoConformidadeOutput> Get(Guid idNaoConformidade, Guid id);
    Task Update(Guid idNaoConformidade, Guid idDefeitoNaoConformidade, DefeitoNaoConformidadeInput input);
    Task Insert (Guid idNaoConformidade, DefeitoNaoConformidadeInput input);
    Task Remove(Guid idNaoConformidade, Guid idDefeitoNaoConformidade);
}