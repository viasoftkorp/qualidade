using System;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Enums;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Dtos;

public class NaoConformidadeViewOutput
{
    public Guid Id { get; set; }
    public Guid IdNaoConformidade { get; set; }
    public int Codigo { get; set; }
    public OrigemNaoConformidade Origem { get; set; }
    public StatusNaoConformidade Status { get; set; }
    public Guid? IdNotaFiscal { get; set; }
    public string NumeroNotaFiscal { get; set; }
    public Guid IdNatureza { get; set; }
    public string DescricaoNatureza { get; set; }
    public int? CodigoNatureza { get; set; }
    public string Natureza { get; set; }
    public Guid? IdCliente { get; set; }
    public string DescricaoCliente { get; set; }
    public string CodigoCliente { get; set; }
    public string Cliente { get; set; }
    public Guid? IdFornecedor { get; set; }
    public string DescricaoFornecedor { get; set; }
    public string CodigoFornecedor { get; set; }
    public string Fornecedor { get; set; }
    public int? NumeroOdf { get; set; }
    public Guid IdProduto { get; set; }
    public string DescricaoProduto { get; set; }
    public string CodigoProduto { get; set; }
    public string Produto { get; set; }
    public string Revisao { get; set; }
    public string Equipe { get; set; }
    public Guid? IdLote { get; set; }
    public string NumeroLote { get; set; }
    public DateTime? DataFabricacaoLote { get; set; }
    public string CampoNf { get; set; }
    public Guid IdCriador { get; set; }
    public bool LoteTotal { get; set; }
    public bool LoteParcial { get; set; }
    public bool Rejeitado { get; set; }
    public bool AceitoConcessao { get; set; }
    public bool RetrabalhoPeloCliente { get; set; }
    public bool RetrabalhoNoCliente { get; set; }
    public bool NaoConformidadeEmPotencial { get; set; }
    public bool RelatoNaoConformidade { get; set; }
    public bool MelhoriaEmPotencial { get; set; }
    public string Descricao { get; set; }
    public string NomeUsuarioCriador { get; set; }
    public string SobrenomeUsuarioCriador { get; set; }
}