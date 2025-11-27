namespace Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Dtos;

public class RelatorioNaoConformidade
{
    public string CodigoRnc { get; set; }
    public string Origem { get; set; }
    public string Data { get; set; }
    public string NotaFiscal { get; set; }
    public string Cliente { get; set; }
    public string Fornecedor { get; set; }
    public string Produto { get; set; }
    public string Lote { get; set; }
    public string CodigoCliente { get; set; }
    public string CodigoFornecedor { get; set; }
    public string CodigoInterno { get; set; }
    public string Revisao { get; set; }
    public bool LoteTotal { get; set; }
    public bool LoteParcial { get; set; }
    public bool Rejeitado { get; set; }
    public bool AceitoConcessao { get; set; }
    public bool RetrabalhadoPeloCliente { get; set; }
    public bool RetrabalhadoNoCliente { get; set; }
    public string TimeEnvolvido { get; set; }
    public bool NaoConformidadePotencial { get; set; }
    public bool NaoConformidade { get; set; }
    public bool MelhoriaPotencial { get; set; }
    public string DescricaoNaoConformidade { get; set; }
    public string ReclamacaoProcedente { get; set; }
    public string ReclamacaoImprocedente { get; set; }
    public string QuantidadeLote { get; set; }
    public string QuantidadeNaoConformidade { get; set; }
    public string AprovadoReclamacao { get; set; }
    public string ConcessaoReclamacao { get; set; }
    public string RejeitadoReclamacao { get; set; }
    public string Retrabalho { get; set; }
    public bool RetrabalhoComOnus { get; set; }
    public bool RetrabalhoSemOnus { get; set; }
    public bool DevolucaoFornecedor { get; set; }
    public bool Recodificar { get; set; }
    public bool Sucata { get; set; }
    public string Observacoes { get; set; }
    public bool Reclamacoes { get; set; }
    public bool Conclusoes { get; set; }
    public string Evidencia { get; set; }
    public bool Eficaz { get; set; }
    public string ClicloDeTempo { get; set; }
    public string NomeUsuarioCriador { get; set; }
    public string SobrenomeUsuarioCriador { get; set; }
    public string DataCriacao { get; set; }
    public string EmpresaId { get; set; }
    public string EmpresaLegacyId { get; set; }
    public string EmpresaCnpj { get; set; }
    public string EmpresaRazaoSocial { get; set; }
    public string EmpresaFantasia { get; set; }

    public RelatorioNaoConformidade()
    {
        
    }
}