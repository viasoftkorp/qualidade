using System;
using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ReclamacoesNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ReclamacoesNaoConformidades.Services;

public interface IReclamacaoNaoConformidadeService
{
    Task Insert(Guid idNaoConformidade, ReclamacaoNaoConformidadeInput input);
    Task Update(Guid idNaoConformidade, ReclamacaoNaoConformidadeInput input);
    Task<ReclamacaoNaoConformidadeOutput> Get(Guid idNaoConformidade);
}