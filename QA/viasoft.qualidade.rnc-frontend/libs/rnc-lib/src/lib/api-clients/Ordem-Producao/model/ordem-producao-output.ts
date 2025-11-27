export interface OrdemProducaoOutput {
  numeroOdf: number;
  revisao: string;
  idProduto: string;
  idPedido: string;
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
}
