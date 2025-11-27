using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Enums;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Dtos;

public class NaoConformidadeInput
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
    public string NumeroLote { get; set; }
    
    public int? NumeroOdf { get; set; }
    public int? NumeroOdfRetrabalho { get; set; }
    public Guid? IdOdfRetrabalho { get; set; }
    public string NumeroPedido { get; set; }
    public int? NumeroOdfFaturamento { get; set; }
    public Guid? IdProdutoFaturamento { get; set; }
    public List<Guid> IdsCentrosCusto { get; set; } = new List<Guid>();
    public bool Incompleta { get; set; }


    public NaoConformidadeInput()
    {
        
    }

    public NaoConformidadeInput(NaoConformidadeOutput naoConformidadeOutput)
    {
         Id = naoConformidadeOutput.Id; 
         Codigo = naoConformidadeOutput.Codigo; 
         Origem = naoConformidadeOutput.Origem; 
         Status = naoConformidadeOutput.Status; 
         IdNotaFiscal = naoConformidadeOutput.IdNotaFiscal; 
         IdNatureza = naoConformidadeOutput.IdNatureza; 
         IdPessoa = naoConformidadeOutput.IdPessoa; 
         IdProduto = naoConformidadeOutput.IdProduto; 
         IdLote = naoConformidadeOutput.IdLote; 
         DataFabricacaoLote = naoConformidadeOutput.DataFabricacaoLote; 
         CampoNf = naoConformidadeOutput.CampoNf; 
         IdCriador = naoConformidadeOutput.IdCriador; 
         Revisao = naoConformidadeOutput.Revisao; 
         LoteTotal = naoConformidadeOutput.LoteTotal; 
         LoteParcial = naoConformidadeOutput.LoteParcial; 
         Rejeitado = naoConformidadeOutput.Rejeitado; 
         AceitoConcessao = naoConformidadeOutput.AceitoConcessao; 
         RetrabalhoPeloCliente = naoConformidadeOutput.RetrabalhoPeloCliente; 
         RetrabalhoNoCliente = naoConformidadeOutput.RetrabalhoNoCliente; 
         Equipe = naoConformidadeOutput.Equipe; 
         NaoConformidadeEmPotencial = naoConformidadeOutput.NaoConformidadeEmPotencial; 
         RelatoNaoConformidade = naoConformidadeOutput.RelatoNaoConformidade; 
         MelhoriaEmPotencial = naoConformidadeOutput.MelhoriaEmPotencial; 
         Descricao = naoConformidadeOutput.Descricao; 
         NumeroLote = naoConformidadeOutput.NumeroLote;
         NumeroOdfRetrabalho = naoConformidadeOutput.NumeroOdfRetrabalho;
         IdOdfRetrabalho = naoConformidadeOutput.IdOdfRetrabalho;
         NumeroPedido = naoConformidadeOutput.NumeroPedido;
         NumeroOdfFaturamento = naoConformidadeOutput.NumeroOdfFaturamento;
         IdProdutoFaturamento = naoConformidadeOutput.IdProdutoFaturamento;
         Incompleta = naoConformidadeOutput.Incompleta;
    }
}