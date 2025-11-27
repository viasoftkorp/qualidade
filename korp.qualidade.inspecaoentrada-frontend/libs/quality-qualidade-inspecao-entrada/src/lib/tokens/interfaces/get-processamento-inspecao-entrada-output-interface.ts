import { MovimentarInspecaoStatus } from '../enums';

export interface GetAllProcessamentoInspecaoEntradaOutput {
  items: ProcessamentoInspecaoEntradaOutput[];
  totalCount: number;
}

export interface ProcessamentoInspecaoEntradaOutput {
  idSaga: string;
  status: MovimentarInspecaoStatus;
  erro: string;
  numeroRetentativas: number;
  numeroExecucoes: number;
  quantidadeTotal: number;
  resultado: string;
  codigoProduto: string;
  descricaoProduto: string;
  notaFiscal: number;
  idUsuarioExecucao: string;
  nomeUsuarioExecucao: string;
  dataExecucao?: Date;
  lote: string;
  estorno: boolean;
  transferencias: ProcessamentoInspecaoEntradaTransferenciaOutput[];
}

export interface ProcessamentoInspecaoEntradaTransferenciaOutput {
  numeroPedido: number;
  quantidade: number;
  localDestino: number;
  localOrigem: number;
  descricaoLocalDestino: string;
  descricaoLocalOrigem: string
  tipoTransferencia: number;
}
