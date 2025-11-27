using System;
using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Services;

public interface INaoConformidadeService
{
    Task<NaoConformidadeValidationResult> Create(NaoConformidadeInput input);
    Task<NaoConformidadeValidationResult> Update(Guid id, NaoConformidadeInput input);
    Task Delete(Guid id);
    Task<NaoConformidadeOutput> Get(Guid idNaoConformidade);
}