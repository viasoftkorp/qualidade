using System;
using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Services;

public interface IImplementacaoEvitarReincidenciaNaoConformidadeService
{
    public Task<ImplementacaoEvitarReincidenciaNaoConformidadeOutput> GetById(Guid id);
    public Task Insert(Guid idNaoConformidade, ImplementacaoEvitarReincidenciaNaoConformidadeInput naoConformidadeInput);
    public Task Update(Guid idNaoConformidade, ImplementacaoEvitarReincidenciaNaoConformidadeInput naoConformidadeInput);
    
    public Task Remove(Guid idNaoConformidade, Guid idImplementacaoEvitarReincidencia);

}