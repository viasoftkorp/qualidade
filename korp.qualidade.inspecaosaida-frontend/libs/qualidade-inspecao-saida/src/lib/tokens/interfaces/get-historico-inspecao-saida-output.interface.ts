export interface GetAllHistoricoInspecaoSaidaOutput {
  items: HistoricoInspecaoSaidaOutput[];
  totalCount: number;
}

export interface HistoricoInspecaoSaidaOutput {
  ordemFabricacao: number;
  odfApontada: number;
  codigoProduto: string;
  descricaoProduto: string;
  quantidadeLote: number;
  quantidadeInspecao: number;
}

export interface GetAllHistoricoInspecaoSaidaItensOutput {
  items: HistoricoInspecaoSaidaItensOutput[];
  totalCount: number;
}

export interface HistoricoInspecaoSaidaItensOutput {
  codigoInspecao: number;
  recnoInspecao: number;
  odfApontada: number;
  ordemFabricacao: number;
  codigoProduto: string;
  descricaoProduto: string;
  quantidadeInspecao: number;
  quantidadeRetrabalhada: number;
  quantidadeAprovada: number;
  quantidadeReprovada: number;
  inspetor: string;
  tipoInspecao: string;
  resultado: string;
  dataInspecao: any;
  odfRetrabalho?: number;
  codigoRnc?: number;
  idRnc?: string;
  transferencias: HistoricoInspecaoSaidaTransferenciaOutput[];
}

export interface HistoricoInspecaoSaidaTransferenciaOutput {
  ordemFabricacao: number;
  quantidade: number;
  numeroPedido: string;
  localOrigem: number;
  descricaoLocalOrigem: string;
  localDestino: number;
  descricaoLocalDestino: string;
  tipoTransferencia: number;
}
