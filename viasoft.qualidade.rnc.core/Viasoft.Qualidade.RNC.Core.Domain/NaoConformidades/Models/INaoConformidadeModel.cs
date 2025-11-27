using System;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models;

public interface INaoConformidadeModel
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
    public int? NumeroOdfFaturamento { get; set; }
    public Guid? IdProdutoFaturamento { get; set; }
    
    public Guid CompanyId { get; set; }
    
    public bool Incompleta { get; set; }
    public string NumeroPedido { get; set; }
    
}