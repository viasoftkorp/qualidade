using System;
using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ConclusoesNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ConclusoesNaoConformidades.Services;

public interface IConclusaoNaoConformidadeService
{
    public Task ConcluirNaoConformidade(Guid idNaoConformidade,ConclusaoNaoConformidadeInput input);
    public Task<int> CalcularCicloTempo(Guid idNaoConformidade);
    public Task<ConclusaoNaoConformidadeOutput> Get(Guid idNaoConformidade);
    public Task Estornar(Guid idNaoConformidade);
}