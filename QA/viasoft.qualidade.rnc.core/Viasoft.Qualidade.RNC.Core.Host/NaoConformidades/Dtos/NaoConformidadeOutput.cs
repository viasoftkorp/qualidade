using System;
using Newtonsoft.Json;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;

public class NaoConformidadeOutput : INaoConformidadeModel
{
    public Guid Id { get; set; }
    public int? Codigo { get; set; }
    public OrigemNaoConformidade Origem { get; set; }
    public StatusNaoConformidade Status { get; set; }
    public Guid? IdNotaFiscal { get; set; }
    public Guid IdNatureza { get; set; }
    public Guid? IdPessoa { get; set; }
    public Guid IdProduto { get; set; }
    public Guid? IdLote { get; set; }
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
    public string NumeroNotaFiscal { get; set; }
    public int? NumeroOdf { get; set; }
    public string NumeroLote { get; set; }
    public int? NumeroOdfFaturamento { get; set; }
    public Guid? IdProdutoFaturamento { get; set; }
    public Guid CompanyId { get; set; }
    public bool Incompleta { get; set; }
    public string NumeroPedido { get; set; }
    
    public OperacaoRetrabalhoNaoConformidadeOutput OperacaoRetrabalho { get; set; }

    public NaoConformidadeOutput(NaoConformidade naoConformidade)
    {
        Id = naoConformidade.Id;
        Codigo = naoConformidade.Codigo;
        Origem = naoConformidade.Origem;
        Status = naoConformidade.Status;
        IdNotaFiscal = naoConformidade.IdNotaFiscal;
        IdNatureza = naoConformidade.IdNatureza;
        IdPessoa = naoConformidade.IdPessoa;
        IdProduto = naoConformidade.IdProduto;
        IdLote = naoConformidade.IdLote;
        DataFabricacaoLote = naoConformidade.DataFabricacaoLote;
        CampoNf = naoConformidade.CampoNf;
        IdCriador = naoConformidade.IdCriador;
        Revisao = naoConformidade.Revisao;
        LoteTotal = naoConformidade.LoteTotal;
        LoteParcial = naoConformidade.LoteParcial;
        Rejeitado = naoConformidade.Rejeitado;
        AceitoConcessao = naoConformidade.AceitoConcessao;
        RetrabalhoPeloCliente = naoConformidade.RetrabalhoPeloCliente;
        RetrabalhoNoCliente = naoConformidade.RetrabalhoNoCliente;
        Equipe = naoConformidade.Equipe;
        NaoConformidadeEmPotencial = naoConformidade.NaoConformidadeEmPotencial;
        RelatoNaoConformidade = naoConformidade.RelatoNaoConformidade;
        MelhoriaEmPotencial = naoConformidade.MelhoriaEmPotencial;
        Descricao = naoConformidade.Descricao;
        NumeroLote = naoConformidade.NumeroLote;
        NumeroNotaFiscal = naoConformidade.NumeroNotaFiscal;
        NumeroOdf = naoConformidade.NumeroOdf;
        NumeroPedido = naoConformidade.NumeroPedido;
        NumeroOdfFaturamento = naoConformidade.NumeroOdfFaturamento;
        IdProdutoFaturamento = naoConformidade.IdProdutoFaturamento;
        Incompleta = naoConformidade.Incompleta;
        DataCriacao = naoConformidade.DataCriacao;
        CompanyId = naoConformidade.CompanyId;
        if (naoConformidade.OperacaoRetrabalho != null)
        {
            OperacaoRetrabalho =
                new OperacaoRetrabalhoNaoConformidadeOutput(naoConformidade.OperacaoRetrabalho);
        }
    }

    
}