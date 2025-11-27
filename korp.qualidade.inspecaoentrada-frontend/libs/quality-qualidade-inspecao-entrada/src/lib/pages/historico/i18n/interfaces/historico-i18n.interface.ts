import { IKeyTranslate } from '@viasoft/common';

export interface HistoricoInspecaoI18N extends IKeyTranslate {
  HistoricoInspecao: {
    Metricas: string;
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
    Odf: string;
    LocalOrigem: string;
    LocalDestino: string;
    UsuarioExecucao: string;
    DataExecucao: string;
    CodigoProduto: string;
    DescricaoProduto: string;
    QuantidadeLote: string;
    QuantidadeInspecao: string;
    DataInspecao: string;
    Inspetor: string;
    QuantidadeRetrabalhada: string;
    QuantidadeAprovada: string;
    QuantidadeReprovada: string;
    Lote: string;
    Rnc: string;
    MovimentarInspecaoStatus: {
      Inicio: string;
      EmProcesso: string;
      Falha: string;
      Sucesso: string;
    }
    Estornar: string;
    Transferencia: {
      TipoTransferencia: string;
      TipoTransferencias: {
        Aprovado: string;
        Reprovado: string;
        Retrabalhado: string;
      };
      Quantidade: string;
      DescricaoLocalOrigem: string;
      DescricaoLocalDestino: string;
      NumeroPedido: string;
    }
  };
}
