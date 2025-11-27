using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;

public class NaoConformidade : FullAuditedEntity, IMustHaveEnvironment, IMustHaveTenant
{
    public int? Codigo { get; set; }
    public OrigemNaoConformidade Origem { get; set; }
    public StatusNaoConformidade Status { get; set; }
    public Guid? IdNotaFiscal { get; set; }
    public string NumeroNotaFiscal { get; set; }
    public Guid IdNatureza { get; set; }
    public Guid? IdPessoa { get; set; }
    public int? NumeroOdf { get; set; }
    public Guid IdProduto { get; set; }
    public Guid? IdLote { get; set; }
    public string NumeroLote { get; set; }
    public DateTime? DataFabricacaoLote { get; set; }
    public string CampoNf { get; set; } 
    public Guid IdCriador { get; set; }
    public DateTime DataCriacao { get; set; }
    public string Revisao { get; set; }
    public bool LoteTotal { get; set; }
    public bool LoteParcial { get; set; }
    public bool Rejeitado { get; set; }
    public bool AceitoConcessao { get; set; }
    public bool RetrabalhoPeloCliente { get; set; }
    public bool RetrabalhoNoCliente { get; set; }
    public string Equipe { get; set; } 
    public bool NaoConformidadeEmPotencial { get; set; }
    public bool RelatoNaoConformidade { get; set; }
    public bool MelhoriaEmPotencial { get; set; }
    public string Descricao { get; set; } 
    public Guid EnvironmentId { get; set; }
    public Guid TenantId { get; set; }
    public Guid CompanyId { get; set; }
    public string NumeroPedido { get; set; }
    public int? NumeroOdfFaturamento { get; set; }
    public Guid? IdProdutoFaturamento { get; set; }
    public bool Incompleta { get; set; }
    
    public OperacaoRetrabalhoNaoConformidade OperacaoRetrabalho { get; set; }

    
    public NaoConformidade()
    {
    }

    public NaoConformidade(INaoConformidadeModel model)
    {
        Codigo = model.Codigo;
        Descricao = model.Descricao;
        Equipe = model.Equipe;
        Id = model.Id;
        Origem = model.Origem;
        Rejeitado = model.Rejeitado;
        Revisao = model.Revisao;
        Status = model.Status;
        AceitoConcessao = model.AceitoConcessao;
        CampoNf = model.CampoNf;
        IdPessoa = model.IdPessoa;
        IdLote = model.IdLote;
        NumeroLote = model.NumeroLote;
        NumeroOdf = model.NumeroOdf;
        IdNatureza = model.IdNatureza;
        IdProduto = model.IdProduto;
        LoteParcial = model.LoteParcial;
        LoteTotal = model.LoteTotal;
        DataFabricacaoLote = model.DataFabricacaoLote;
        IdNotaFiscal = model.IdNotaFiscal;
        MelhoriaEmPotencial = model.MelhoriaEmPotencial;
        RelatoNaoConformidade = model.RelatoNaoConformidade;
        RetrabalhoNoCliente = model.RetrabalhoNoCliente;
        RetrabalhoPeloCliente = model.RetrabalhoPeloCliente;
        NaoConformidadeEmPotencial = model.NaoConformidadeEmPotencial;
        NumeroPedido = model.NumeroPedido;
        NumeroOdfFaturamento = model.NumeroOdfFaturamento;
        IdProdutoFaturamento = model.IdProdutoFaturamento;
        Incompleta = model.Incompleta;
    }
    
}
