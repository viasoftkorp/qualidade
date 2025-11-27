using System;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Qualidade.RNC.Core.Domain.AcaoPreventivaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.AcoesPreventivas;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Usuarios;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;

public class AcaoPreventivaNaoConformidadeViewOutput
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public Guid IdDefeitoNaoConformidade { get; set; }
    public Guid IdAcaoPreventiva { get; set; }
    public string Descricao { get; set; }
    public int Codigo { get; set; }
    public string Acao { get; set; }
    public string Responsavel { get; set; }
    public string Auditor { get; set; }
    [IsArrayOfBytes]
    public string Detalhamento { get; set; }
    public Guid? IdResponsavel { get; set; }
    public DateTime? DataAnalise { get; set; }
    public DateTime? DataPrevistaImplantacao { get; set; }
    public Guid? IdAuditor { get; set; }
    public bool Implementada { get; set; }
    public DateTime? DataVerificacao { get; set; }
    public DateTime? NovaData { get; set; }

    public AcaoPreventivaNaoConformidadeViewOutput()
    {
    }

    public AcaoPreventivaNaoConformidadeViewOutput(AcaoPreventivaNaoConformidade acaoPreventivaNaoConformidade, AcaoPreventiva acaoPreventiva, Usuario usuario)
    {
        Id = acaoPreventivaNaoConformidade.Id;
        IdNaoConformidade = acaoPreventivaNaoConformidade.IdNaoConformidade;
        IdDefeitoNaoConformidade = acaoPreventivaNaoConformidade.IdDefeitoNaoConformidade;
        IdAcaoPreventiva = acaoPreventivaNaoConformidade.IdAcaoPreventiva;
        Descricao = acaoPreventiva.Descricao;
        Codigo = acaoPreventiva.Codigo;
        Responsavel = $"{usuario.Nome} {usuario.Sobrenome}"; 
        Auditor = $"{usuario.Nome} {usuario.Sobrenome}";
        Acao = acaoPreventivaNaoConformidade.Acao;
        Detalhamento = acaoPreventivaNaoConformidade.Detalhamento;
        IdResponsavel = acaoPreventivaNaoConformidade.IdResponsavel;
        DataAnalise = acaoPreventivaNaoConformidade.DataAnalise;
        DataPrevistaImplantacao = acaoPreventivaNaoConformidade.DataPrevistaImplantacao;
        IdAuditor = acaoPreventivaNaoConformidade.IdAuditor;
        Implementada = acaoPreventivaNaoConformidade.Implementada;
        DataVerificacao = acaoPreventivaNaoConformidade.DataVerificacao;
        NovaData = acaoPreventivaNaoConformidade.NovaData;
    }
}
