import { IKeyTranslate } from '@viasoft/common';

export interface HistoricoInspecaoI18N extends IKeyTranslate {
  HistoricoInspecao: {
    Transferencias: string;
    Metricas: string
    Filtrar: string;
    Filtros: string;
    Limpar: string;
    Status: string;
    Lote: string;
    NumeroExecucoes: string;
    NumeroRetentativas: string;
    Erro: string;
    Resultado: string;
    Quantidade: string;
    QuantidadeTotal: string;
    Produto: string;
    NumeroPedido: string;
    Odf: string;
    OdfRetrabalho: string;
    CodigoRnc: string;
    Rnc: string;
    LocalOrigem: string;
    LocalDestino: string;
    UsuarioExecucao: string;
    DataExecucao: string;
    CodigoProduto: string;
    DescricaoProduto: string;
    QuantidadeLote: string;
    QuantidadeInspecao: string;
    TipoInspecao: string;
    DataInspecao: string;
    Inspetor: string;
    QuantidadeRetrabalhada: string;
    QuantidadeAprovada: string;
    QuantidadeReprovada: string;
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
