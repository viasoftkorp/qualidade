import { NotaFiscalDTO } from './get-notas-fiscais-dto.interface';

export interface GetAllHistoricoInspecaoEntradaOutput {
  items: NotaFiscalDTO[];
  totalCount: number;
}

export interface GetAllHistoricoInspecaoEntradaItensOutput {
  items: HistoricoInspecaoEntradaItensOutput[];
  totalCount: number;
}

export interface HistoricoInspecaoEntradaItensOutput {
  idRnc?: string;
  recnoInspecao: number;
  codigoInspecao: number;
  notaFiscal: number;
  codigoProduto: string;
  descricaoProduto: string;
  quantidadeInspecao: number;
  quantidadeRetrabalhada: number;
  quantidadeAprovada: number;
  quantidadeReprovada: number;
  inspetor: string;
  resultado: string;
  dataInspecao: any;
  transferencias: HistoricoInspecaoEntradaTransferenciaOutput[];
}

export interface HistoricoInspecaoEntradaTransferenciaOutput {
  notaFiscal: number;
  quantidade: number;
  numeroPedido: string;
  localOrigem: number;
  descricaoLocalOrigem: string;
  localDestino: number;
  descricaoLocalDestino: string;
  tipoTransferencia: number;
}
