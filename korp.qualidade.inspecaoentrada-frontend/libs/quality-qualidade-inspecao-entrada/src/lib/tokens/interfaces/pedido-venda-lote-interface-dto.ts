export interface PedidoVendaLoteDto {
  id: string;
  numeroLote: string;
  quantidade: number;
  idInspecaoEntradaPedidoVenda: string;
}

export interface GetPedidoVendaLoteDto {
  items: PedidoVendaLoteDto[];
  totalCount: number;
}
