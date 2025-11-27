import { MovimentarInspecaoStatus } from '../enums';

export interface GetAllProcessamentoInspecaoSaidaOutput {
  items: ProcessamentoInspecaoSaidaOutput[];
  totalCount: number;
}

export interface ProcessamentoInspecaoSaidaOutput {
  idSaga: string;
  status: MovimentarInspecaoStatus;
  erro: string;
  numeroRetentativas: number;
  numeroExecucoes: number;
  quantidadeTotal: number;
  resultado: string;
  codigoProduto: string;
  descricaoProduto: string;
  odf: number;
  idUsuarioExecucao: string;
  nomeUsuarioExecucao: string;
  dataExecucao?: Date;
  lote: string;
  estorno: boolean;
  transferencias: ProcessamentoInspecaoSaidaTransferenciaOutput[];
}

export interface ProcessamentoInspecaoSaidaTransferenciaOutput {
  ordemFabricacao: number;
  quantidade: number;
  numeroPedido: string;
  localOrigem: number;
  descricaoLocalOrigem: string;
  localDestino: number;
  descricaoLocalDestino: string;
  tipoTransferencia: number;
}
