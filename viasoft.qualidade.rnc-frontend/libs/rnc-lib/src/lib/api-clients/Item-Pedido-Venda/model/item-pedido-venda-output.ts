export interface ItemPedidoVendaOutput {
  id: string;
  odf?: number;
  campoNotaFiscal: string;
  revisao: string;
  codigoProduto: string;
  idProduto: string;
  idEmpresa: string;
  numeroPedido: string;
  isOdfRetrabalho: boolean;
  odfOrigem?: number;
  numeroLote: string;
  quantidade: number;
}
