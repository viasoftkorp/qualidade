using Viasoft.Qualidade.RNC.Core.Domain.ConclusaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.ConclusaoNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ConclusoesNaoConformidades.Dtos;

public class ConclusaoNaoConformidadeOutput : ConclusaoNaoConformidadeModel
{
    public ConclusaoNaoConformidadeOutput(ConclusaoNaoConformidade conclusao)
    {
        Id = conclusao.Id;
        IdNaoConformidade = conclusao.IdNaoConformidade;
        Eficaz = conclusao.Eficaz;
        DataReuniao = conclusao.DataReuniao;
        DataVerificacao = conclusao.DataVerificacao;
        NovaReuniao = conclusao.NovaReuniao;
        IdAuditor = conclusao.IdAuditor;
        Evidencia = conclusao.Evidencia;
        CicloDeTempo = conclusao.CicloDeTempo;
        IdNovoRelatorio = conclusao.IdNovoRelatorio;
    }
}