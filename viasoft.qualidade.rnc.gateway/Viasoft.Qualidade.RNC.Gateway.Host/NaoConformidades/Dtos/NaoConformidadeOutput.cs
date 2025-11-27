using System;
using Newtonsoft.Json;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacoesRetrabalhoNaoConformidade.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Dtos;

public class NaoConformidadeOutput
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
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
    public int? NumeroOdfRetrabalho { get; set; }
    public Guid? IdOdfRetrabalho { get; set; }
    public string NumeroPedido { get; set; }
    public int? NumeroOdfFaturamento { get; set; }
    public Guid? IdProdutoFaturamento { get; set; }
    public bool Incompleta { get; set; }
    public DateTime DataCriacao { get; set; }
    public Guid CompanyId { get; set; }

}