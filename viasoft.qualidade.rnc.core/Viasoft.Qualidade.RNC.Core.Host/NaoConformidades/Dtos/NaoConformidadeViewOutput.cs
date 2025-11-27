using System;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Usuarios;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Domain.Naturezas;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;

public class NaoConformidadeViewOutput
{
    public Guid Id { get; set; }
    public int? Codigo { get; set; }
    public OrigemNaoConformidade Origem { get; set; }
    public StatusNaoConformidade Status { get; set; }
    public Guid? IdNotaFiscal { get; set; }
    public string NumeroNotaFiscal { get; set; }
    public Guid IdNatureza { get; set; }
    public string DescricaoNatureza { get; set; }
    public int CodigoNatureza { get; set; }
    public string Natureza { get; set; }
    public Guid? IdCliente { get; set; }
    public string NomeCliente { get; set; }
    public string CodigoCliente { get; set; }
    public string Cliente { get; set; }
    public Guid?  IdFornecedor { get; set; }
    public string NomeFornecedor { get; set; }
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
    public bool Incompleta { get; set; }
    public string NomeUsuarioCriador { get; set; }
    public string SobrenomeUsuarioCriador { get; set; }

    public NaoConformidadeViewOutput()
    {
    }

    public NaoConformidadeViewOutput(Natureza natureza, Produto produto, NaoConformidade naoConformidade,
        Usuario usuario)
    {
        Id = naoConformidade.Id;
        Codigo = naoConformidade.Codigo;
        Origem = naoConformidade.Origem;
        Status = naoConformidade.Status;
        IdNotaFiscal = naoConformidade.IdNotaFiscal;
        NumeroNotaFiscal = naoConformidade.NumeroNotaFiscal;
        IdNatureza = natureza.Id;
        DescricaoNatureza = natureza.Descricao;
        CodigoNatureza = natureza.Codigo;
        Natureza = $"{natureza.Codigo} - {natureza.Descricao}";
        NumeroOdf = naoConformidade.NumeroOdf;
        IdProduto = produto.Id;
        DescricaoProduto = produto.Descricao;
        CodigoProduto = produto.Codigo;
        Produto = $"{produto.Codigo} - {produto.Descricao}";
        Revisao = naoConformidade.Revisao;
        Equipe = naoConformidade.Equipe;
        IdLote = naoConformidade.IdLote;
        NumeroLote = naoConformidade.NumeroLote;
        DataFabricacaoLote = naoConformidade.DataFabricacaoLote;
        CampoNf = naoConformidade.CampoNf;
        IdCriador = usuario.Id;
        NomeUsuarioCriador = usuario.Nome;
        SobrenomeUsuarioCriador = usuario.Sobrenome;
        LoteTotal = naoConformidade.LoteTotal;
        LoteParcial = naoConformidade.LoteParcial;
        Rejeitado = naoConformidade.Rejeitado;
        AceitoConcessao = naoConformidade.AceitoConcessao;
        RetrabalhoPeloCliente = naoConformidade.RetrabalhoPeloCliente;
        RetrabalhoNoCliente = naoConformidade.RetrabalhoNoCliente;
        NaoConformidadeEmPotencial = naoConformidade.NaoConformidadeEmPotencial;
        RelatoNaoConformidade = naoConformidade.RelatoNaoConformidade;
        MelhoriaEmPotencial = naoConformidade.MelhoriaEmPotencial;
        Descricao = naoConformidade.Descricao;
        Incompleta = naoConformidade.Incompleta;
    }
}