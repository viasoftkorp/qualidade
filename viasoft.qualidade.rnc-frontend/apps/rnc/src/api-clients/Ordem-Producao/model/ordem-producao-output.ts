export interface OrdemProducaoOutput {
  numeroOdf: number;
  revisao: string;
  idProduto: string;
  quantidade: number;
  dataInicio: Date;
  dataEntrega: Date;
  numeroPedido: string;
  observacao: string;
  isRetrabalho: boolean;
  numeroOdfDestino: number | null;
  numeroOdfFaturamento: number;
  idProdutoFaturamento: string;
  numeroLote: string;
  odfFinalizada: boolean;
  possuiPartida: boolean;
  idCliente:string;
}
