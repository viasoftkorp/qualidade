import { IKeyTranslate } from '@viasoft/common';

export interface ProcessamentoInspecaoI18N extends IKeyTranslate {
  ProcessamentoInspecao: {
    Lote: string;
    Transferencias: string;
    Filtrar: string;
    Filtros: string;
    Limpar: string;
    Status: string;
    NumeroExecucoes: string;
    NumeroRetentativas: string;
    Erro: string;
    Resultado: string;
    Quantidade: string;
    QuantidadeTotal: string;
    Produto: string;
    NumeroPedido: string;
    NotaFiscal: string;
    LocalOrigem: string;
    LocalDestino: string;
    UsuarioExecucao: string;
    DataExecucao: string;
    LiberarProcessamento: string;
    MovimentarInspecaoStatus: {
      Inicio: string;
      EmProcesso: string;
      Falha: string;
      Sucesso: string;
    }
    Reprocessar: string;
    Inspecionados: string;
    Estornados: string;
    Transferencia: {
      TipoTransferencia: string;
      TipoTransferencias: {
        Aprovado: string;
        Reprovado: string;
        Retrabalhado: string;
      };
      Quantidade: string;
      NumeroPedido: string;
      DescricaoLocalOrigem: string;
      DescricaoLocalDestino: string;
    }
  };
}
